//using Confluent.Kafka;
//using Domain.Interfaces.Facade;

//namespace Kafka.Facade
//{
//    public class KafkaFacade : IKafkaFacade
//    {
//        //private IKafkaConsumerBuilder<string, string> kafkaConsumer;
//        private IConsumer<string, string> consumer;
//        private bool disposedValue;

//        public ConsumeResult<string, string> Consume(CancellationToken cancellationToken)
//        {
//            var consumerMessage = consumer.Consume(cancellationToken);

//            return consumerMessage;
//        }

//        public void Commit(ConsumeResult<string, string> consumeResult) => consumer.Commit(consumeResult);

//        public void Subscribe(string topic)
//        {
//            consumer.Subscribe(topic);
//        }

//        public IKafkaFacade CreateInstance(ConsumerConfig consumerConfig)
//        {
//            //kafkaConsumer = new IKafkaConsumerBuilder<string, string>(consumerConfig);
//            return this;
//        }

//        public void BuildConsumer()
//        {
//            consumer = kafkaConsumer.Build();
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    consumer?.Dispose();
//                    kafkaConsumer = null;
//                }
//                disposedValue = true;
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }
//    }
//}
