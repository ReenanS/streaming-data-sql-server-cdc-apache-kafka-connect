using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.Diagnostics;

namespace Worker.Extensions
{
    public static class HealthChecksExtensions
    {
        public static void AddHealthCheckService(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<KafkaConsumerHealthCheck>("Kafka Consumer Health Check");
        }

        public static void RegisterHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthCheck("/health");
        }

    }
}
