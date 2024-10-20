using Domain.Interfaces;

namespace Infra.Services
{
    public class MessageProcessor : IMessageRepository
    {
        public string ConvertOperation(string operation)
        {
            return operation switch
            {
                "c" => "criar",
                "u" => "atualizar",
                "d" => "deletar",
                "r" => "leitura",
                _ => throw new ArgumentException($"Operação desconhecida: {operation}")
            };
        }

        public int GetPrimaryKey(dynamic payload, string topic)
        {
            dynamic data = payload.After ?? payload.Before;

            return topic switch
            {
                "joaozinho.DBCN502.dbo.CONBE007" => data.ID_CONBE007,
                "renanzinho.DBCN502.dbo.CONBE008" => data.ID_CONBE008,
                "tiophon.DBCN502.dbo.CONBE009" => data.ID_CONBE009,
                _ => throw new ArgumentException($"Tópico {topic} não suportado")
            };
        }
    }

}
