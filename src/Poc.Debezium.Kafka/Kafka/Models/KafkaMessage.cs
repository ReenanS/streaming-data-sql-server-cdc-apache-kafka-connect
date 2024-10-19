using System.Text.Json.Serialization;

namespace Kafka.Models
{
    public class KafkaMessage
    {
        [JsonPropertyName("payload")]
        public Payload payload { get; set; }

        [JsonPropertyName("ts_ms")]
        public long Timestamp { get; set; }  // Timestamp do Kafka

        public class Payload
        {
            [JsonPropertyName("before")]
            public dynamic Before { get; set; }  // Contém o estado antes da operação (null em caso de 'create')

            [JsonPropertyName("after")]
            public dynamic After { get; set; }   // Contém o estado após a operação (null em caso de 'delete')

            [JsonPropertyName("source")]
            public Source Source { get; set; }   // Metadados da origem

            [JsonPropertyName("op")]
            public string Op { get; set; }       // Tipo de operação: "c" (create), "u" (update), "d" (delete)

            [JsonPropertyName("ts_ms")]
            public long TsMs { get; set; }       // Timestamp do evento
        }

        public class Source
        {
            [JsonPropertyName("version")]
            public string Version { get; set; }   // Versão do conector

            [JsonPropertyName("connector")]
            public string Connector { get; set; } // Nome do conector (ex: "sqlserver")

            [JsonPropertyName("name")]
            public string Name { get; set; }      // Nome da instância do banco de dados

            [JsonPropertyName("db")]
            public string Db { get; set; }        // Nome do banco de dados

            [JsonPropertyName("schema")]
            public string Schema { get; set; }    // Nome do schema

            [JsonPropertyName("table")]
            public string Table { get; set; }     // Nome da tabela onde ocorreu a operação

            [JsonPropertyName("snapshot")]
            public string Snapshot { get; set; }  // Se o evento é proveniente de um snapshot (true/false)

            [JsonPropertyName("change_lsn")]
            public string ChangeLsn { get; set; } // Log sequence number (LSN) de mudança (usado no SQL Server)

            [JsonPropertyName("commit_lsn")]
            public string CommitLsn { get; set; } // LSN de commit (usado no SQL Server)

            [JsonPropertyName("event_serial_no")]
            public long EventSerialNo { get; set; } // Número serial do evento
        }
    }
}
