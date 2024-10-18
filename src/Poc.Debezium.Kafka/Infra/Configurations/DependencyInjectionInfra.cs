using Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Configurations
{
    public static class DependencyInjectionInfra
    {
        public static IServiceCollection AddInfra(this IServiceCollection services)
        {
            //services.AddScoped<ISecretsManagerService, SecretsManagerService>();
            return services;
        }
    }
}
