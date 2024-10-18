using Confluent.Kafka;
using Domain.Interfaces.BackgroundTask;
using Domain.Interfaces.Facade;
using Kafka.Configuration;
using Kafka.Configurations;
using Kafka.Models;
using Worker.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Worker.Services
{
    public class KafkaConsumerService : BackgroundService
    {

        public KafkaConsumerService()
        {
            InitializeKafkaConfiguration();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            ReadMessageFromKafka(stoppingToken);
        }

        public async Task ReadMessageFromKafka(CancellationToken stoppingToken)
        {
            try
            {
                KafkaConsumerHealthCheck.IsKafkaConsumerHealthy = true;


            }
        }

    }
}
