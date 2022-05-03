using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule
{
    public class SetConditionRequest
    {
        [
            Required, 
            EnumDataType(typeof(ConditionFieldType))
        ]
        public string ConditionField { get; set; }
        
        [
            Required,
            EnumDataType(typeof(LogLevel)),
            StringLength(100, MinimumLength = 1)
        ]
        public string Value { get; set; }

        public SetConditionRequest()
        {
        }

        public SetConditionRequest(NotificationConditionDto item)
        {
            ConditionField = item.ConditionField.ToString();
            Value = item.Value;
        }

        public void Fill(SetConditionRequest from)
        {
            ConditionField = from.ConditionField;
            Value = from.Value;
        }
    }
}
