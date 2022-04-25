using System;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class NotificationConditionDto : IResponse
    {
        public long Id { get; set; }
        
        public virtual ConditionFieldType ConditionField { get; set; }
        
        public virtual string Value { get; set; }
        
        public virtual DateTime CreateTime { get; set; }
        
        public virtual DateTime UpdateTime { get; set; }
    }
}
