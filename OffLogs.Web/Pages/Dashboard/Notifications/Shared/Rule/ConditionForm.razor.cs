using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Shared.Rule;

public partial class ConditionForm
{
    [Parameter]
    public SetConditionRequest Condition { get; set; }

    [CascadingParameter]
    protected NotificationRuleForm? Parent { get; set; }
    
    [Parameter]
    public EventCallback<SetConditionRequest> OnDeleted { get; set; }
    
    private ICollection<DropDownListItem> _fieldTypeDownListItems => ConditionFieldType.LogLevel.ToDropDownList();
    private ICollection<DropDownListItem> _logLevelDownListItems => LogLevel.Warning.ToDropDownList();
    
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

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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

    private Task OnChangeFieldAsync(string field)
    {
        _model.ConditionField = field;

        UpdateParentModel();
        return Task.CompletedTask;
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
