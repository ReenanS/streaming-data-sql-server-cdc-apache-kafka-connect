using System.Text.Json.Serialization;

namespace Kafka.Models
{
    public class KafkaMessage
    {
        [JsonPropertyName("payload")]
        public Payload payload { get; set; }

        public class Payload
        {
            [JsonPropertyName("before")]
            public dynamic Before { get; set; }  // Contém o estado antes da operação (null em caso de 'create')

            [JsonPropertyName("after")]
            public dynamic After { get; set; }   // Contém o estado após a operação (null em caso de 'delete')

            [JsonPropertyName("op")]
            public string Op { get; set; }       // Tipo de operação: "c" (create), "u" (update), "d" (delete)

        }

    }
}
