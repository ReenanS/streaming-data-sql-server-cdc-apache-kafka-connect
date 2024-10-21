using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Domain.Models;
using Infra.Configurations;
using Infra.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Infra.Services
{
    public class SendOperationToStepFunctionServiceTests
    {
        private readonly Mock<IAmazonStepFunctions> _mockAmazonStepFunctions;
        private readonly Mock<ILogger<SendOperationToStepFunctionService>> _mockLogger;
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly SendOperationToStepFunctionService _service;

        public SendOperationToStepFunctionServiceTests()
        {
            _mockAmazonStepFunctions = new Mock<IAmazonStepFunctions>();
            _mockLogger = new Mock<ILogger<SendOperationToStepFunctionService>>();

            _kafkaConfiguration = new KafkaConfiguration
            {
                Topics = new List<TopicConfig>
                {
                    new TopicConfig { Topic = "test-topic", StateMachineArn = "arn:aws:states:us-east-1:123456789012:stateMachine:testStateMachine" }
                }
            };

            _service = new SendOperationToStepFunctionService(_mockLogger.Object, _mockAmazonStepFunctions.Object, _kafkaConfiguration);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            // Arrange
            string topic = "test-topic";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ExecuteAsync(null, topic));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenStateMachineArnNotFound()
        {
            // Arrange
            var input = new StepFunctionInput { TipoOperacao = "criar", CodigoExterno = 1 };
            string invalidTopic = "invalid-topic";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ExecuteAsync(input, invalidTopic));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeStepFunction_WhenValidInput()
        {
            // Arrange
            var input = new StepFunctionInput { TipoOperacao = "criar", CodigoExterno = 1 };
            string topic = "test-topic";

            // Mock de resposta do StartExecutionAsync
            var mockResponse = new StartExecutionResponse
            {
                ExecutionArn = "arn:aws:states:us-east-1:123456789012:execution:testStateMachine:testExecution",
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };

            _mockAmazonStepFunctions
                .Setup(x => x.StartExecutionAsync(It.IsAny<StartExecutionRequest>(), default))
                .ReturnsAsync(mockResponse);

            // Act
            await _service.ExecuteAsync(input, topic);

            // Assert
            _mockAmazonStepFunctions.Verify(
                x => x.StartExecutionAsync(It.IsAny<StartExecutionRequest>(), default),
                Times.Once
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenStepFunctionExecutionFails()
        {
            // Arrange
            var input = new StepFunctionInput { TipoOperacao = "criar", CodigoExterno = 1 };
            string topic = "test-topic";

            // Mock para lançar exceção
            _mockAmazonStepFunctions
                .Setup(x => x.StartExecutionAsync(It.IsAny<StartExecutionRequest>(), default))
                .ThrowsAsync(new Exception("AWS Step Function failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.ExecuteAsync(input, topic));

            // Verifica se a exceção contém a mensagem esperada
            Assert.Equal("AWS Step Function failed", exception.Message);
        }

        [Fact]
        public void GetStateMachineArnByTopic_ShouldReturnCorrectArn_WhenTopicExists()
        {
            // Act
            string arn = _service.GetType()
                                 .GetMethod("GetStateMachineArnByTopic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                 ?.Invoke(_service, new object[] { "test-topic" }) as string;

            // Assert
            Assert.Equal("arn:aws:states:us-east-1:123456789012:stateMachine:testStateMachine", arn);
        }

        [Fact]
        public void GetStateMachineArnByTopic_ShouldReturnNull_WhenTopicDoesNotExist()
        {
            // Act
            string arn = _service.GetType()
                                 .GetMethod("GetStateMachineArnByTopic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                 ?.Invoke(_service, new object[] { "invalid-topic" }) as string;

            // Assert
            Assert.Null(arn);
        }
    }
}
