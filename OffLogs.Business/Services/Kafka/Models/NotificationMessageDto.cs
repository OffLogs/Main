using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Kafka.Models
{
    public class NotificationMessageDto : IKafkaDto 
        
    {
        public string ContextType { get; set; }
        public string ContextData { get; set; }

        public NotificationMessageDto() { }

        public static NotificationMessageDto Create<TContext>(TContext context) where TContext : INotificationContext
        {
            return new NotificationMessageDto()
            {
                ContextType = context.GetTypeAsString(),
                ContextData = Newtonsoft.Json.JsonConvert.SerializeObject(context)
            };
        }

        public object GetDeserializedData(Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(ContextData, type);
        }
    }
}
