using Confluent.Kafka;
using Kafka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.Services
{
    public class KafkaConsumerManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly KafkaConfiguration kafkaConfiguration;

        public KafkaConsumerManager(IServiceProvider serviceProvider, KafkaConfiguration kafkaConfiguration)
        {
            this.serviceProvider = serviceProvider;
            this.kafkaConfiguration = kafkaConfiguration;
        }

        public IConsumer<string, string> CreateConsumer(string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaConfiguration.BootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<string, string>(config).Build();
        }
    }

}
