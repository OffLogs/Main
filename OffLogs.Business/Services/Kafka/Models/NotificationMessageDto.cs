using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffLogs.Business.Helpers;

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
                ContextData = JsonHelper.SerializeToString(context)
            };
        }

        public TConext GetDeserializedData<TConext>() where TConext: INotificationContext
        {
            return JsonHelper.DeserializeObject<TConext>(ContextData);
        }
    }
}
