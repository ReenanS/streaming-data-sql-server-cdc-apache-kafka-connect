using Domain.Interfaces;
using Domain.Interfaces.UseCases;
using Domain.Models;

namespace Application.Commands
{
    public class ProcessKafkaMessageCommandHandler
    {
        private readonly IMessageProcessor _messageRepository;
        private readonly ISendOperationToStepFunction _sendOperationToStepFunction;

        public ProcessKafkaMessageCommandHandler(IMessageProcessor messageRepository, ISendOperationToStepFunction sendOperationToStepFunction)
        {
            _messageRepository = messageRepository;
            _sendOperationToStepFunction = sendOperationToStepFunction;
        }

        public async Task Handle(ProcessKafkaMessageCommand command, CancellationToken cancellationToken)
        {
            // Obter o identificador a partir do payload e do tópico
            int id = _messageRepository.GetPrimaryKey(command.KafkaMessage.payload, command.Topic);

            // Processar a operação com base no id obtido
            var operation = _messageRepository.ConvertOperation(command.KafkaMessage.payload.Op);

            // Criar o input para a Step Function
            var input = new StepFunctionInput
            {
                TipoOperacao = operation,
                CodigoExterno = id,
                StateMachineArn = "TESTE RENANZINHO" // Obter do comando ou de outro lugar
            };

            // Enviar para a Step Function
            await _sendOperationToStepFunction.ExecuteAsync(input);
        }
    }
}
