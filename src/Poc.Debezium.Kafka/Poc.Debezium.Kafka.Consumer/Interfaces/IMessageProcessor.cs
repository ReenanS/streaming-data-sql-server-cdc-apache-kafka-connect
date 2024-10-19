namespace Worker.Interfaces
{
    public interface IMessageProcessor
    {
        string ConvertOperation(string operation);
        int GetPrimaryKey(dynamic payload, string topic);
    }

}
