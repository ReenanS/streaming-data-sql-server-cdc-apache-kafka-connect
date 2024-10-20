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
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                // Iniciar a tarefa de consumo para cada tópico
                _ = Task.Run(() => ConsumeFromTopic(consumerConfig, topicConfig.Topic, stoppingToken), stoppingToken);
            }
        }

        private async Task ConsumeFromTopic(ConsumerConfig config, string topic, CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            // Inicializando o contador de mensagens
            int messageCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var fixMessage = consumeResult?.Message?.Value;

                    // Verificar se a mensagem não é nula
                    if (string.IsNullOrEmpty(fixMessage))
                    {
                        Console.WriteLine($"Mensagem vazia ou nula recebida do tópico {topic}, ignorando...");
                        continue;
                    }

                    // Incrementa o contador de mensagens recebidas
                    messageCount++;
                    Console.WriteLine($"Mensagem {messageCount} recebida do tópico {topic}");

                    // Deserializar a mensagem Kafka, com verificação de nulidade no KafkaMessage
                    var kafkaMessage = JsonConvert.DeserializeObject<KafkaMessage>(fixMessage);
                    if (kafkaMessage?.payload == null)
                    {
                        Console.WriteLine("Payload nulo, ignorando a mensagem...");
                        continue;
                    }

                    // Verificar a operação e os objetos Before/After de acordo com a operação
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

                    string stateMachineArn = kafkaConfiguration.Topics.FirstOrDefault(t => t.Topic == topic)?.StateMachineArn;

                    var input = new StepFunctionInput
                    {
                        TipoOperacao = operation,
                        CodigoExterno = id,
                        StateMachineArn = stateMachineArn
                    };

                    // Adicionar trabalho à fila de tarefas
                    taskQueue.QueueBackgroundWorkItem(async (token) =>
                    {
                        int maxRetries = 3; // número máximo de tentativas
                        int delayBetweenRetries = 2000; // tempo de espera entre tentativas em milissegundos
                        bool success = false;

                        for (int attempt = 1; attempt <= maxRetries; attempt++)
                        {
                            try
                            {
                                using var scope = serviceProvider.CreateScope();
                                //var sendOperationToStepFunctionAsync = scope.ServiceProvider.GetRequiredService<ISendOperationToStepFunction>();
                                //await sendOperationToStepFunctionAsync.ExecuteAsync(input);
                                consumer.Commit(consumeResult);
                                success = true; // sucesso ao processar a mensagem
                                break; // sai do loop se a mensagem for processada com sucesso
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao processar mensagem (tentativa {attempt}): {ex.Message}");

                                // Se não for a última tentativa, aguarda antes de tentar novamente
                                if (attempt < maxRetries)
                                {
                                    await Task.Delay(delayBetweenRetries, token);
                                }
                                else
                                {
                                    // Lógica de falha após todas as tentativas (opcional)
                                    Console.WriteLine("Todas as tentativas falharam. Considerando a mensagem como não processada.");
                                }
                            }
                        }
                    });
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Erro ao consumir mensagem do tópico {topic}: {e.Error.Reason}");
                    await Task.Delay(1000, stoppingToken); // Espera um pouco antes de tentar novamente
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado: {ex.Message}");
                    await Task.Delay(1000, stoppingToken); // Espera um pouco antes de tentar novamente
                }
            }

            // Exibir o total de mensagens processadas (opcional)
            Console.WriteLine($"Total de mensagens recebidas do tópico {topic}: {messageCount}");
        }


    }
}
