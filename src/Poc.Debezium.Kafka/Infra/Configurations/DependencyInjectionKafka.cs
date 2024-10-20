using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infra.Configurations
{
    public static class DependencyInjectionKafka
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration.GetSection(KafkaConfiguration.DEFAULT_CONFIG_SECTION))
                .AddScoped(c => c.GetService<IOptionsSnapshot<KafkaConfiguration>>().Value);

            //services.AddScoped<IKafkaFacade, KafkaFacade>();

            return services;
        }
    }
}
