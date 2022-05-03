using System;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class NotificationConditionDto : IResponse
    {
        public long Id { get; set; }
        
        public ConditionFieldType ConditionField { get; set; }
        
        public string Value { get; set; }
        
        public DateTime CreateTime { get; set; }
        
        public DateTime UpdateTime { get; set; }
    }
}
