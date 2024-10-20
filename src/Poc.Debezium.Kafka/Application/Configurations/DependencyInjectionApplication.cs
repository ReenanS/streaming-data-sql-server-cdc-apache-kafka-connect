using Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations
{
    public static class DependencyInjectionApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registrar seus handlers específicos
            services.AddTransient<ProcessKafkaMessageCommandHandler>();
            return services;
        }
    }
}
