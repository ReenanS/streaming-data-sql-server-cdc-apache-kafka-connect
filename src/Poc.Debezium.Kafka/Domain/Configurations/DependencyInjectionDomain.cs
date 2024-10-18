using Domain.Interfaces.UseCases;
using Domain.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
