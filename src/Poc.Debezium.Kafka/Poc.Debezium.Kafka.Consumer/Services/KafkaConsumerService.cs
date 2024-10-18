using Confluent.Kafka;
using Domain.Interfaces.BackgroundTask;
//using Domain.Interfaces.Facade;
using Kafka.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Worker.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> consumer;
        private readonly IBackgroundTaskQueue taskQueue;
        private IServiceScopeFactory serviceProvider;
        private readonly KafkaConfiguration kafkaConfiguration;

        private ConsumerConfig consumerConfig;

        public KafkaConsumerService(IServiceScopeFactory serviceProvider,
            IConsumer<string, string> consumer,
            IBackgroundTaskQueue taskQueue,
            IOptions<KafkaConfiguration> kafkaConfiguration)
        {
            this.consumer = consumer;
            this.taskQueue = taskQueue;
            this.serviceProvider = serviceProvider;

            this.kafkaConfiguration = kafkaConfiguration.Value;

            //InitializeKafkaConfiguration();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            ReadMessageFromKafka(stoppingToken);
        }

        public async Task ReadMessageFromKafka(CancellationToken stoppingToken)
        {
            try
            {
                //KafkaConsumerHealthCheck.IsKafkaConsumerHealthy = true;

                //KafkaFacade.CreateInstance(consumerConfig).BuildConsumer();
                consumer.Subscribe(kafkaConfiguration.Topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var fixMessage = consumeResult.Message.Value;

                    JObject cdcMessage = JObject.Parse(fixMessage);

                    Console.WriteLine(fixMessage);

                    taskQueue.QueueBackgroundWorkItem(async (token) =>
                    {
                        try
                        {
                            using var scope = serviceProvider.CreateScope();
                            consumer.Commit(consumeResult);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao processar mensagem");
                        }
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no consumer do Kafka");
            }
            finally
            {
                consumer.Dispose();
            }
        }
    }
}
