//using Confluent.Kafka;

//namespace Domain.Interfaces.Facade
//{
//    public interface IKafkaFacade : IDisposable
//    {
//        ConsumeResult<string, string> Consume(CancellationToken cancellationToken);
//        void Commit(ConsumeResult<string, string> consumeResult);
//        void Subscribe(string topic);
//        IKafkaFacade CreateInstance(ConsumerConfig consumerConfig);
//        void BuildConsumer();
//    }
//}
