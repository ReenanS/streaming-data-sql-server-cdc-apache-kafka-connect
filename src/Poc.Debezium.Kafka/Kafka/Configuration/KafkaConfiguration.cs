using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configuration
{
    public class KafkaConfiguration
    {
        public static readonly string DEFAULT_CONFIG_SECTION = "Kafka";
        public string DefaultBootstrapServer {  get; set; }
        public List<KafkaTopicSettings> Topics { get; set; }
    }

    public class KafkaTopicSettings
    {
        public string Name { get; set; }
        public string GroupId { get; set; }
        public string ClientId { get; set; }
    }
}
