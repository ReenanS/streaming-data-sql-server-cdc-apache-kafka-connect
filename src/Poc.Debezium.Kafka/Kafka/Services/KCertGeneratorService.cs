using Confluent.Kafka;
using Kafka.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kafka.Services
{
    public class KCertGeneratorService : IHostedService
    {
        private readonly KaaSCertConfiguration kCertConfiguration;
        private readonly Serilog.ILogger logger;
        private readonly ILogger<KaaSCertClientManager> loggerKaas;
    }

    public KCertGeneratorService()
    {

    }

    public async Task StartAsync()
    {

    }
}
