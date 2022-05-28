using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Rules.Parts;

public partial class ConditionForm
{
    [Parameter]
    public SetConditionRequest Condition { get; set; }

    [CascadingParameter]
    protected EditRuleForm? Parent { get; set; }
    
    [Parameter]
    public EventCallback<SetConditionRequest> OnDeleted { get; set; }

    private SetConditionRequest _model = new();

    private string _value
    {
        get => _model.Value;
        set
        {
            _model.Value = value;
            UpdateParentModel();
        }
    }

    private ConditionFieldType _conditionField
    {
        get
        {
            if (string.IsNullOrEmpty(_model.ConditionField))
            {
                return default;
            }

            return Enum.Parse<ConditionFieldType>(_model.Value);
        }
        set
        {
            _model.Value = value.ToString();
            UpdateParentModel();
        }
    }
    
    private LogLevel _logLevel
    {
        get
        {
            if (Enum.TryParse<LogLevel>(_model.Value, out var logLevel))
            {
                return logLevel;
            }
            return default;
        }
        set
        {
            _model.Value = value.ToString();
            UpdateParentModel();
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (Condition != null)
        {
            _model = Condition;
            _model.ConditionField = ConditionFieldType.LogLevel.ToString();
        }
    }

    private void UpdateParentModel()
    {
        var model = Parent?.Model.Conditions.FirstOrDefault(
            item => item == _model
        );
        model?.Fill(_model);
    }
    
    private void OnDelete()
    {
        InvokeAsync(async () => await OnDeleted.InvokeAsync(_model));
    }
}
