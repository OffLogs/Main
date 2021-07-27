using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Kafka.Models
{
    public class NotificationMessageDto<TConext> where TConext : INotificationContext, IKafkaDto
    {
        public string ContextType { get; set; }
        public TConext NotificationContext { get; set; }
    }
}
