namespace Kafka.Configuration
{
    public class KafkaConfiguration
    {
        public static readonly string DEFAULT_CONFIG_SECTION = "Kafka";
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }
        public string AutoOffsetReset { get; set; }
    }

    public class KafkaTopicSettings
    {
        public string Name { get; set; }
        public string GroupId { get; set; }
        public string ClientId { get; set; }
    }
}
