namespace Worker.Interfaces
{
    public interface IMessageProcessor
    {
        string ConvertOperation(string operation);
        //string GetPrimaryKey(KafkaPayload payload, string topic);
    }

}
