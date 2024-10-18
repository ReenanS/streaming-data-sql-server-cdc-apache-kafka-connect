namespace Kafka.Models
{
    public class KafkaMessage
    {
        public BeforeAfterData? before { get; set; }
        public BeforeAfterData? after { get; set; }
        public string op { get; set; }

        public class BeforeAfterData
        {
            public int? ID_Grupo { get; set; }
            public int? ID_Bem { get; set; }
            public int? ID_CONGR001C { get; set; }
            public string? SN_Venda { get; set; }
            public string? SN_Troca_Bem { get; set; }
            public int? ID_CONGR001G { get; set; }
            public string? SN_Sincroniza_Lista { get; set; }
        }

    }
}
