using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Notification;
using Radzen;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class NotificationTemplatesDropDown
{
    [Parameter]
    public long Value
    {
        get => _selectedId;
        set => _selectedId = value;
    }
    
    [Parameter]
    public EventCallback<long> ValueChanged { get; set; }

    [Parameter]
    public EventCallback<MessageTemplateDto> SelectedItemChanged { get; set; }
    
    [Parameter]
    public string Placeholder { get; set; } = CommonResources.SelectMessageTemplate;
    
    [Parameter]
    public string Class { get; set; }
    
    [Inject]
    public IState<NotificationRuleState> _state { get; set; }
    
    private MessageTemplateDto _selectedItem;

    private long _selectedId = 0;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(new FetchMessageTemplatesAction(true));
    }

    private void OnValueChanged(long selectedId)
    {
        _selectedItem = _state.Value.MessageTemplates.FirstOrDefault(item => item.Id == selectedId);
        SelectedItemChanged.InvokeAsync(_selectedItem);
        ValueChanged.InvokeAsync(selectedId);
    }
}
