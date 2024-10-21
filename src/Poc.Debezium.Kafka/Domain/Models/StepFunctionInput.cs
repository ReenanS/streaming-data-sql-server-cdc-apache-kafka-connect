using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class StepFunctionInput
    {
        [JsonPropertyName("tipo_operacao")]
        public string TipoOperacao { get; set; }

        [JsonPropertyName("codigo_externo")]
        public int CodigoExterno { get; set; }
    }
}
