namespace Kafka.Models
{
    public class KafkaMessage
    {
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        public string Op { get; set; }
        public Dictionary<string, object> After { get; set; }
    }
}
