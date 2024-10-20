using Domain.Models;

namespace Domain.Interfaces.UseCases
{
    public interface ISendOperationToStepFunction
    {
        Task ExecuteAsync(StepFunctionInput input);
    }
}
