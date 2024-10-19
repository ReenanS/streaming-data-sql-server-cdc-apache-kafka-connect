using Worker.Interfaces;

namespace Worker.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        public string ConvertOperation(string operation)
        {
            return operation switch
            {
                "c" => "criar",
                "u" => "atualizar",
                "d" => "deletar",
                _ => throw new ArgumentException($"Operação desconhecida: {operation}")
            };
        }

        //public string GetPrimaryKey(KafkaPayload payload, string topic)
        //{
        //    // Aqui você pode implementar a lógica para obter a chave primária com base no payload e no tópico.
        //    // A implementação pode variar dependendo de como suas tabelas estão estruturadas.

        //    return topic switch
        //    {
        //        "CONBE007" => payload.IdConbe007,
        //        "CONBE008" => payload.IdConbe008,
        //        _ => throw new ArgumentException($"Tópico desconhecido: {topic}")
        //    };
        //}
    }

}
