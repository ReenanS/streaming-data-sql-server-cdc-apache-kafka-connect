using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Configurations
{
    public static class DependencyInjectionDomain
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            //services.AddScoped<ISendOperationToStepFunction, SendOperationToStepFunction>();
            return services;
        }
    }
}
