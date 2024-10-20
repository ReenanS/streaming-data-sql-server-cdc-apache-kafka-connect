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
        private readonly ILogger<SendOperationToStepFunctionService> _logger;
        private readonly IAmazonStepFunctions _amazonStepFunctions;
        private readonly KafkaConfiguration _kafkaConfiguration;

        public SendOperationToStepFunctionService(
            ILogger<SendOperationToStepFunctionService> logger,
            IAmazonStepFunctions amazonStepFunctions,
            KafkaConfiguration kafkaConfiguration)
        {
            _logger = logger;
            _amazonStepFunctions = amazonStepFunctions;
            _kafkaConfiguration = kafkaConfiguration;
        }

        public async Task ExecuteAsync(StepFunctionInput input, string topic)
        {
            // Validar entrada
            if (input == null)
            {
                _logger.LogError("Input nulo fornecido para execução da Step Function.");
                throw new ArgumentNullException(nameof(input));
            }

            // Buscar o ARN da Step Function baseado no tópico Kafka
            var stateMachineArn = GetStateMachineArnByTopic(topic);

            if (string.IsNullOrEmpty(stateMachineArn))
            {
                _logger.LogError("Nenhuma State Machine ARN configurada para o tópico {topic}.", topic);
                throw new InvalidOperationException($"Nenhuma State Machine ARN configurada para o tópico {topic}");
            }

            // Log de informações sobre o input recebido e a ARN da Step Function
            _logger.LogInformation("Iniciando execução da Step Function para o tópico {topic} com os seguintes dados: TipoOperacao: {tipoOperacao}, CodigoExterno: {codigoExterno}, StateMachineArn: {stateMachineArn}",
                topic, input.TipoOperacao, input.CodigoExterno, stateMachineArn);

            // Configurar a requisição de execução da Step Function
            var startExecutionRequest = new StartExecutionRequest
            {
                Input = JsonSerializer.Serialize(input),
                StateMachineArn = stateMachineArn
            };

            try
            {
                // Disparar execução da Step Function
                var response = await _amazonStepFunctions.StartExecutionAsync(startExecutionRequest);

                // Logar o sucesso da execução
                _logger.LogInformation("Execução da Step Function iniciada com sucesso. ExecutionArn: {executionArn}, Estado: {status}",
                    response.ExecutionArn, response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                // Log de erro em caso de exceção ao invocar Step Function
                _logger.LogError(ex, "Erro ao iniciar execução da Step Function para o tópico {topic}.", topic);
                throw;
            }
        }

        /// <summary>
        /// Obtém o ARN da State Machine associado ao tópico Kafka.
        /// </summary>
        /// <param name="topic">Nome do tópico Kafka.</param>
        /// <returns>O ARN da State Machine configurada para o tópico.</returns>
        private string GetStateMachineArnByTopic(string topic)
        {
            // Procura o tópico na configuração do Kafka
            var topicConfig = _kafkaConfiguration.Topics.FirstOrDefault(t => t.Topic == topic);

            if (topicConfig == null)
            {
                _logger.LogWarning("Tópico {topic} não encontrado na configuração do Kafka.", topic);
                return null;
            }

            return topicConfig.StateMachineArn;
        }

    }
}
