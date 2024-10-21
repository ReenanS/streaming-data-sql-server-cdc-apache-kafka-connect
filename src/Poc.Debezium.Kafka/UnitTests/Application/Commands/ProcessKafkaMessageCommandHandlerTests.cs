namespace UnitTests.Application.Commands
{
    using Domain.Interfaces;
    using Domain.Interfaces.UseCases;
    using Domain.Models;
    using global::Application.Commands;
    using Kafka.Models;
    using Moq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static Kafka.Models.KafkaMessage;

    public class ProcessKafkaMessageCommandHandlerTests
    {
        private readonly Mock<IMessageProcessor> _mockMessageProcessor;
        private readonly Mock<ISendOperationToStepFunction> _mockSendOperationToStepFunction;
        private readonly ProcessKafkaMessageCommandHandler _handler;

        public ProcessKafkaMessageCommandHandlerTests()
        {
            // Mocks
            _mockMessageProcessor = new Mock<IMessageProcessor>();
            _mockSendOperationToStepFunction = new Mock<ISendOperationToStepFunction>();

            // Instância do handler com os mocks
            _handler = new ProcessKafkaMessageCommandHandler(
                _mockMessageProcessor.Object,
                _mockSendOperationToStepFunction.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldProcessMessageCorrectly()
        {
            // Arrange
            var kafkaMessage = new KafkaMessage
            {
                payload = new Payload { Op = "C", After = "{ \"Id\": 123 }" }
            };
            var command = new ProcessKafkaMessageCommand(kafkaMessage, "test-topic");

            // Mockar a obtenção da chave primária
            _mockMessageProcessor.Setup(x => x.GetPrimaryKey(kafkaMessage.payload, "test-topic"))
                .Returns(123);

            // Mockar a conversão da operação
            _mockMessageProcessor.Setup(x => x.ConvertOperation(kafkaMessage.payload.Op))
                .Returns("Create");

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMessageProcessor.Verify(x => x.GetPrimaryKey(kafkaMessage.payload, "test-topic"), Times.Once);
            _mockMessageProcessor.Verify(x => x.ConvertOperation(kafkaMessage.payload.Op), Times.Once);

            // Verificar que a Step Function foi chamada corretamente
            _mockSendOperationToStepFunction.Verify(x =>
                x.ExecuteAsync(It.Is<StepFunctionInput>(input =>
                    input.TipoOperacao == "Create" &&
                    input.CodigoExterno == 123
                ), "test-topic"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldHandleDifferentOperationType()
        {
            // Arrange
            var kafkaMessage = new KafkaMessage
            {
                payload = new Payload { Op = "U", After = "{ \"Id\": 456 }" }
            };
            var command = new ProcessKafkaMessageCommand(kafkaMessage, "test-topic");

            // Mockar a obtenção da chave primária
            _mockMessageProcessor.Setup(x => x.GetPrimaryKey(kafkaMessage.payload, "test-topic"))
                .Returns(456);

            // Mockar a conversão da operação
            _mockMessageProcessor.Setup(x => x.ConvertOperation(kafkaMessage.payload.Op))
                .Returns("Update");

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMessageProcessor.Verify(x => x.GetPrimaryKey(kafkaMessage.payload, "test-topic"), Times.Once);
            _mockMessageProcessor.Verify(x => x.ConvertOperation(kafkaMessage.payload.Op), Times.Once);

            // Verificar que a Step Function foi chamada corretamente
            _mockSendOperationToStepFunction.Verify(x =>
                x.ExecuteAsync(It.Is<StepFunctionInput>(input =>
                    input.TipoOperacao == "Update" &&
                    input.CodigoExterno == 456
                ), "test-topic"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowExceptionIfMessageProcessorFails()
        {
            // Arrange
            var kafkaMessage = new KafkaMessage
            {
                payload = new Payload { Op = "D", After = "{ \"Id\": 789 }" }
            };
            var command = new ProcessKafkaMessageCommand(kafkaMessage, "test-topic");

            // Simular erro ao obter a chave primária
            _mockMessageProcessor.Setup(x => x.GetPrimaryKey(kafkaMessage.payload, "test-topic"))
                .Throws(new System.Exception("Erro ao obter a chave primária"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mockMessageProcessor.Verify(x => x.GetPrimaryKey(kafkaMessage.payload, "test-topic"), Times.Once);
            _mockSendOperationToStepFunction.Verify(x => x.ExecuteAsync(It.IsAny<StepFunctionInput>(), It.IsAny<string>()), Times.Never);
        }
    }

}
