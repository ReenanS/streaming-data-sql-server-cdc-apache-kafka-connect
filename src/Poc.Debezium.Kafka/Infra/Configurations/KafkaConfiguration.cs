namespace Infra.Configurations
{
    public class KafkaConfiguration
    {
        public const string DEFAULT_CONFIG_SECTION = "Kafka";
        public string BootstrapServers { get; set; }
        public string AutoOffsetReset { get; set; }
        public List<TopicConfig> Topics { get; set; }
    }

    public class TopicConfig
    {
        public string Topic { get; set; }
        public string GroupId { get; set; }
        public string StateMachineArn { get; set; }
    }
}