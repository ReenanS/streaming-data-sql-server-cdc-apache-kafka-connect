using Kafka.Models;

namespace Application.Commands
{
    public class ProcessKafkaMessageCommand
    {
        public KafkaMessage KafkaMessage { get; }
        public string Topic { get; }

        public ProcessKafkaMessageCommand(KafkaMessage kafkaMessage, string topic)
        {
            KafkaMessage = kafkaMessage;
            Topic = topic;
        }
    }
}
