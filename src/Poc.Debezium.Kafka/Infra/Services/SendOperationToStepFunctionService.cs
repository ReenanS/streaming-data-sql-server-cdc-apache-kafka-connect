using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Domain.Interfaces.UseCases;
using Domain.Models;
using Infra.Configurations;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text.Json;

namespace Infra.Services
{
    public class SendOperationToStepFunctionService : ISendOperationToStepFunction
    {
        private readonly ILogger<SendOperationToStepFunctionService> logger;
        private readonly IAmazonStepFunctions amazonStepFunctions;
        private readonly KafkaConfiguration kafkaConfiguration;

        public SendOperationToStepFunctionService(
            ILogger<SendOperationToStepFunctionService> logger,
            IAmazonStepFunctions amazonStepFunctions,
            KafkaConfiguration kafkaConfiguration)
        {
            this.logger = logger;
            this.amazonStepFunctions = amazonStepFunctions;
            this.kafkaConfiguration = kafkaConfiguration;
        }

        public async Task ExecuteAsync(StepFunctionInput input, string topic)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // Busca o ARN da State Machine correspondente ao tópico
            var stateMachineArn = GetStateMachineArnByTopic(topic);

            if (string.IsNullOrEmpty(stateMachineArn))
            {
                throw new InvalidOperationException($"Nenhuma State Machine ARN configurada para o tópico {topic}");
            }

            logger.LogInformation("Step Function Input - TipoOperacao: {tipoOperacao}, CodigoExterno: {codigoExterno}, StateMachineArn: {stateMachineArn}", input.TipoOperacao, input.CodigoExterno, stateMachineArn);

            var startExecutionRequest = new StartExecutionRequest
            {
                Input = JsonSerializer.Serialize(input),
                StateMachineArn = stateMachineArn
            };

            await amazonStepFunctions.StartExecutionAsync(startExecutionRequest);
        }

        private string GetStateMachineArnByTopic(string topic)
        {
            // Procura o ARN da State Machine com base no tópico
            var topicConfig = kafkaConfiguration.Topics.FirstOrDefault(t => t.Topic == topic);
            return topicConfig?.StateMachineArn;
        }

    }
}
