using Domain.Interfaces.UseCases;
using Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Configurations
{
    public static class DependencyInjectionInfra
    {
        public static IServiceCollection AddInfra(this IServiceCollection services)
        {
            //services.AddScoped<ISecretsManagerService, SecretsManagerService>();
            services.AddScoped<ISendOperationToStepFunction, SendOperationToStepFunctionService>();
            return services;
        }
    }
}
