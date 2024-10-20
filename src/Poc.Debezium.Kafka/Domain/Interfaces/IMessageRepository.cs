namespace Domain.Interfaces
{
    public interface IMessageRepository
    {
        string ConvertOperation(string operation);
        int GetPrimaryKey(dynamic payload, string topic);
    }
}
