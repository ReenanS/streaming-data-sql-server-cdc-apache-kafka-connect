//using Domain.Models;
//using Domain.Interfaces.UseCases;
//using Itau;
//using Microsoft.Extensions.Configuration;
//using Serilog;
//using System.Text.Json;
//using Amazon.StepFunctions;
//using Domain.Interfaces.UseCases;

//namespace Domain.UseCases
//{
//    public class SendOperationToStepFunction : ISendOperationToStepFunction
//    {
//        private readonly ILogger logger;
//        private readonly IAmazonStepFunctions amazonStepFunctions;
//        private readonly string stateMachineArn;

//        public SendOperationToStepFunction(
//            ILogger logger,
//            IConfiguration configuration,
//            IAmazonStepFunctions amazonStepFunctions)
//        {
//            this.logger = logger;
//            this.amazonStepFunctions = amazonStepFunctions;
//            stateMachineArn = configuration["StateMachineArn"];
//        }

//        public async Task ExecuteAsync(string tipoOperacao, int? idGrupo)
//        {
//            var input = new
//            {
//                TipoOperacao = tipoOperacao,
//                GroupId = idGrupo
//            };

//            logger.Information("Step Function Input - TipoOperacao: {tipoOperacao}, GroupId: {idGrupo}", tipoOperacao, idGrupo);
//            logger.Information("State Machine Arn - {stateMachineArn}", stateMachineArn);

//            var startExecutionRequest = new StartExecutionRequest
//            {
//                Input = JsonSerializer.Serialize(input),
//                StateMachineArn = stateMachineArn
//            };

//            var response = await amazonStepFunctions.StartExecutionAsync(startExecutionRequest);
//            logger.Information("Mensagem enviada para Step Function. ExecutionArn: {executionArn}, TipoOperacao: {tipoOperacao}, GroupId: {idGrupo}", response.ExecutionArn, tipoOperacao, idGrupo);
//        }
//    }
//}
