using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafka.Models;

namespace Worker.Interfaces
{
    public interface IMessageProcessor
    {
        string ConvertOperation(string operation);
        //string GetPrimaryKey(KafkaPayload payload, string topic);
    }

}
