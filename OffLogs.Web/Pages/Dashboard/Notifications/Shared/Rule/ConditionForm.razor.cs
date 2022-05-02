using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Web.Core.Extensions;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Shared.Rule;

public partial class ConditionForm
{
    [Parameter]
    public SetConditionRequest Condition { get; set; }

    [Parameter]
    public EventCallback<SetConditionRequest> OnChanged { get; set; }
    
    private ICollection<DropDownListItem> _fieldTypeDownListItems => ConditionFieldType.LogLevel.ToDropDownList(true);
    
    private SetConditionRequest _model = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _model.Value = Condition.Value;
        _model.ConditionField = Condition.ConditionField;
    }
}
