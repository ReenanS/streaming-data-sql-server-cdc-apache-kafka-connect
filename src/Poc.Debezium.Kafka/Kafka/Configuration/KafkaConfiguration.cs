namespace Kafka.Configuration
{
    public class KafkaConfiguration
    {
        public static readonly string DEFAULT_CONFIG_SECTION = "Kafka";
        public string BootstrapServers { get; set; }
        public string AutoOffsetReset { get; set; }
        public List<TopicConfig> Topics { get; set; }
    }

    public class TopicConfig
    {
        public string Topic { get; set; }
        public string GroupId { get; set; }
    }
}