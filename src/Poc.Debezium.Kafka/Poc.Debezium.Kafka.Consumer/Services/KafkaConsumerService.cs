using Confluent.Kafka;
using Domain.Interfaces.BackgroundTask;
using Domain.Interfaces.UseCases;
using Domain.Models;
using Kafka.Configuration;
using Kafka.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Worker.Interfaces;

namespace Worker.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IBackgroundTaskQueue taskQueue;
        private readonly IServiceScopeFactory serviceProvider;
        private readonly KafkaConfiguration kafkaConfiguration;
        private readonly IMessageProcessor messageProcessor;

        public KafkaConsumerService(IServiceScopeFactory serviceProvider,
            IBackgroundTaskQueue taskQueue,
            IOptions<KafkaConfiguration> kafkaConfiguration,
            IMessageProcessor messageProcessor)
        {
            this.taskQueue = taskQueue;
            this.serviceProvider = serviceProvider;
            this.kafkaConfiguration = kafkaConfiguration.Value;
            this.messageProcessor = messageProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            // Criar um consumidor para cada tópico na configuração
            foreach (var topicConfig in kafkaConfiguration.Topics)
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = kafkaConfiguration.BootstrapServers,
                    GroupId = topicConfig.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false // Desativa AutoCommit
                };

                // Iniciar a tarefa de consumo para cada tópico
                _ = Task.Run(() => ConsumeFromTopic(consumerConfig, topicConfig.Topic, stoppingToken), stoppingToken);
            }
        }

        private async Task ConsumeFromTopic(ConsumerConfig config, string topic, CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(config)
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Partições atribuídas: [{string.Join(", ", partitions)}]");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Partições revogadas: [{string.Join(", ", partitions)}]");
                })
                .Build();

            consumer.Subscribe(topic);

            int messageCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    // Verificação de nulidade
                    if (consumeResult?.Message == null || string.IsNullOrEmpty(consumeResult.Message.Value))
                    {
                        Console.WriteLine($"Mensagem vazia ou nula recebida do tópico {topic}, ignorando...");
                        continue;
                    }

                    var fixMessage = consumeResult.Message.Value;

                    // Incrementa o contador de mensagens recebidas
                    messageCount++;
                    Console.WriteLine($"Mensagem {messageCount} recebida do tópico {topic}");

                    // Deserializar a mensagem Kafka
                    var kafkaMessage = JsonConvert.DeserializeObject<KafkaMessage>(fixMessage);
                    if (kafkaMessage?.payload == null)
                    {
                        Console.WriteLine("Payload nulo, ignorando a mensagem...");
                        continue;
                    }

                    // Lógica para obter a operação e validar Before/After para DELETE
                    var operation = messageProcessor.ConvertOperation(kafkaMessage.payload.Op);

                    if (operation == "c" || operation == "u")
                    {
                        if (kafkaMessage.payload.After == null)
                        {
                            Console.WriteLine("Objeto 'After' é nulo, ignorando a mensagem...");
                            continue;
                        }
                    }
                    else if (operation == "d")
                    {
                        if (kafkaMessage.payload.Before == null)
                        {
                            Console.WriteLine("Objeto 'Before' é nulo, ignorando a mensagem...");
                            continue;
                        }
                    }

                    int id = messageProcessor.GetPrimaryKey(kafkaMessage.payload, topic);

                    // Obter o ARN da StateMachine associado ao tópico
                    string stateMachineArn = kafkaConfiguration.Topics.FirstOrDefault(t => t.Topic == topic)?.StateMachineArn;

                    var input = new StepFunctionInput
                    {
                        TipoOperacao = operation,
                        CodigoExterno = id,
                        StateMachineArn = stateMachineArn
                    };

                    // Adicionar trabalho à fila de tarefas com mecanismo de retry
                    taskQueue.QueueBackgroundWorkItem(async (token) =>
                    {
                        int maxRetries = 3;
                        int delayBetweenRetries = 2000;
                        bool success = false;

                        for (int attempt = 1; attempt <= maxRetries; attempt++)
                        {
                            try
                            {
                                using var scope = serviceProvider.CreateScope();
                                //var sendOperationToStepFunctionAsync = scope.ServiceProvider.GetRequiredService<ISendOperationToStepFunction>();
                                //await sendOperationToStepFunctionAsync.ExecuteAsync(input);

                                // Commit manual após processar a mensagem com sucesso
                                consumer.Commit(consumeResult);
                                success = true;
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao processar mensagem (tentativa {attempt}): {ex.Message}");

                                if (attempt < maxRetries)
                                {
                                    await Task.Delay(delayBetweenRetries, token);
                                }
                                else
                                {
                                    Console.WriteLine("Todas as tentativas falharam. Considerando a mensagem como não processada.");
                                }
                            }
                        }
                    });
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Erro ao consumir mensagem do tópico {topic}: {e.Error.Reason}");
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado: {ex.Message}");
                    await Task.Delay(1000, stoppingToken);
                }
            }

            Console.WriteLine($"Total de mensagens recebidas do tópico {topic}: {messageCount}");
        }



    }
}
