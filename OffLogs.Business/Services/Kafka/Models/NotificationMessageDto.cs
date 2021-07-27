using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Kafka.Models
{
    public class NotificationMessageDto<TConext> : IKafkaDto 
        where TConext : INotificationContext
    {
        public string ContextType { get; set; }
        public TConext NotificationContext { get; set; }
    }
}
