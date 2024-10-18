using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configuration
{
    public static class DependencyInjectionKafka
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KaaSCertConfiguration>(configuration.GetSection(KaaSCertConfiguration.DEFAULT_CONFIG_SECTION))
                .AddScoped(c => c.GetService<IOptionsSnapshot<KaaSCertConfiguration>>().Value);

            services.Configure<KafkaConfiguration>(configuration.GetSection(KafkaConfiguration.DEFAULT_CONFIG_SECTION))
                .AddScoped(c => c.GetService<IOptionsSnapshot<KafkaConfiguration>>().Value);

            services.AddScoped<IKafkaFacade, KafkaFacade>();

            return services;
        }
    }
}
