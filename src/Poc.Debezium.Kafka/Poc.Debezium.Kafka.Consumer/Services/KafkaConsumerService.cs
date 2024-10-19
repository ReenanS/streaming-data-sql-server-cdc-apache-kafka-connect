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
            consumer.Subscribe(topic); // Inscreve-se no tópico correto

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var fixMessage = consumeResult.Message.Value;


                    Console.WriteLine($"Mensagem recebida do tópico {topic}");
                    Console.WriteLine($"Mensagem recebida do tópico {topic}: {fixMessage}");

                    var kafkaMessage = JsonConvert.DeserializeObject<KafkaMessage>(fixMessage);
                    //if (kafkaMessage?.payload == null) continue;

                    // Usando a classe de processamento de mensagem
                    string operation = messageProcessor.ConvertOperation(kafkaMessage.payload.Op);

                    int id = messageProcessor.GetPrimaryKey(kafkaMessage.payload, topic);

                    string stateMachineArn = kafkaConfiguration.Topics.FirstOrDefault(t => t.Topic == topic)?.StateMachineArn;

                    var input = new StepFunctionInput
                    {
                        TipoOperacao = operation,
                        GroupId = id,
                        StateMachineArn = stateMachineArn // Passa o ARN aqui
                    };

                    // Adicionar trabalho à fila de tarefas
                    taskQueue.QueueBackgroundWorkItem(async (token) =>
                    {
                        try
                        {
                            using var scope = serviceProvider.CreateScope();

                            //var sendOperationToStepFunctionAsync = scope.ServiceProvider.GetRequiredService<ISendOperationToStepFunction>();

                            // Chama o Step Function
                            //await sendOperationToStepFunctionAsync.ExecuteAsync(input);

                            consumer.Commit(consumeResult);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao processar mensagem: " + ex.Message);
                        }
                    });
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Erro ao consumir mensagem do tópico {topic}: {e.Error.Reason}");
                }
            }
        }
    }
}
