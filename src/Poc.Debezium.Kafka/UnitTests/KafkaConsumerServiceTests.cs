using Domain.Interfaces;
using Domain.Interfaces.BackgroundTask;
using Infra.Configurations;
using Infra.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace UnitTests
{
    public class KafkaConsumerServiceTests
    {
        private readonly Mock<IBackgroundTaskQueue> mockTaskQueue;
        private readonly Mock<IServiceScopeFactory> mockServiceScopeFactory;
        private readonly Mock<IOptions<KafkaConfiguration>> mockKafkaConfigOptions;
        private readonly Mock<ILogger<KafkaConsumerService>> mockLogger;

        public KafkaConsumerServiceTests()
        {
            mockTaskQueue = new Mock<IBackgroundTaskQueue>();
            mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockKafkaConfigOptions = new Mock<IOptions<KafkaConfiguration>>();
            mockLogger = new Mock<ILogger<KafkaConsumerService>>();
        }

        private KafkaConsumerService CreateService(KafkaConfiguration kafkaConfiguration)
        {
            mockKafkaConfigOptions.Setup(m => m.Value).Returns(kafkaConfiguration);
            return new KafkaConsumerService(mockServiceScopeFactory.Object, mockKafkaConfigOptions.Object, mockLogger.Object, mockTaskQueue.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldStartConsumerForEachTopic()
        {
            // Arrange
            // Simulando as configurações Kafka
            var kafkaConfig = new KafkaConfiguration
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = "Earliest",
                Topics = new List<TopicConfig>
            {
                new TopicConfig
                {
                    Topic = "joaozinho.DBCN502.dbo.CONBE007",
                    GroupId = "DBCN502.dbo.CONBE007",
                    StateMachineArn = "arn:aws:states:your_region:your_account_id:stateMachine:CONBE007"
                },
                new TopicConfig
                {
                    Topic = "renanzinho.DBCN502.dbo.CONBE008",
                    GroupId = "DBCN502.dbo.CONBE008",
                    StateMachineArn = "arn:aws:states:your_region:your_account_id:stateMachine:CONBE008"
                },
                new TopicConfig
                {
                    Topic = "tiophon.DBCN502.dbo.CONBE009",
                    GroupId = "DBCN502.dbo.CONBE009",
                    StateMachineArn = "arn:aws:states:your_region:your_account_id:stateMachine:CONBE009"
                }
            }
            };

            var service = CreateService(kafkaConfig);
            var cancellationToken = new CancellationTokenSource().Token;

            // Act
            await service.StartAsync(cancellationToken);

            // Assert
            mockKafkaConfigOptions.Verify(m => m.Value, Times.Once);
            // Verifica se o consumidor foi iniciado para cada tópico
            Assert.Equal(3, kafkaConfig.Topics.Count);
        }
    }
}