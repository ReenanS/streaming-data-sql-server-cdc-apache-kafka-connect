using Infra.Services;

namespace UnitTests.Infra.Services
{
    public class MessageProcessorTests
    {
        private readonly MessageProcessor _messageProcessor;

        public MessageProcessorTests()
        {
            _messageProcessor = new MessageProcessor();
        }

        [Theory]
        [InlineData("c", "criar")]
        [InlineData("u", "atualizar")]
        [InlineData("d", "deletar")]
        [InlineData("r", "leitura")]
        public void ConvertOperation_ShouldReturnCorrectOperation_WhenValidInput(string input, string expected)
        {
            // Act
            var result = _messageProcessor.ConvertOperation(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertOperation_ShouldThrowArgumentException_WhenUnknownOperation()
        {
            // Arrange
            var invalidOperation = "x";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _messageProcessor.ConvertOperation(invalidOperation));
            Assert.Equal($"Operação desconhecida: {invalidOperation}", exception.Message);
        }

    }
}
