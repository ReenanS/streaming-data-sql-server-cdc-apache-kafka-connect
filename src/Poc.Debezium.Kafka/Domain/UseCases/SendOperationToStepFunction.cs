using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Domain.Interfaces.UseCases;
using Domain.Models;
using Serilog;
using System.Text.Json;

namespace Domain.UseCases
{
    public class SendOperationToStepFunction : ISendOperationToStepFunction
    {
        private readonly ILogger logger;
        private readonly IAmazonStepFunctions amazonStepFunctions;

        public SendOperationToStepFunction(
            ILogger logger,
            IAmazonStepFunctions amazonStepFunctions)
        {
            this.logger = logger;
            this.amazonStepFunctions = amazonStepFunctions;
        }

        public async Task ExecuteAsync(StepFunctionInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrEmpty(input.StateMachineArn)) throw new ArgumentException("StateMachineArn não pode ser nulo ou vazio.", nameof(input.StateMachineArn));

            logger.Information("Step Function Input - TipoOperacao: {tipoOperacao}, GroupId: {groupId}, StateMachineArn: {stateMachineArn}", input.TipoOperacao, input.GroupId, input.StateMachineArn);

            var startExecutionRequest = new StartExecutionRequest
            {
                Input = JsonSerializer.Serialize(input),
                StateMachineArn = input.StateMachineArn // Utiliza a propriedade StateMachineArn do input
            };

            var response = await amazonStepFunctions.StartExecutionAsync(startExecutionRequest);
            logger.Information("Mensagem enviada para Step Function. ExecutionArn: {executionArn}, TipoOperacao: {tipoOperacao}, GroupId: {groupId}", response.ExecutionArn, input.TipoOperacao, input.GroupId);
        }
    }
}
