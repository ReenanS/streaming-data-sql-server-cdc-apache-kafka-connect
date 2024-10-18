using Domain.Interfaces.BackgroundTask;
using Worker.BackgroundTask;

namespace Worker.Configurations
{
    public static class DepedencyInjectionWorker
    {
        public static IServiceCollection AddWorker(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            //services.AddSingleton<KafkaConsumerHealthCheck>();

            return services;
        }
    }
}
