using System.Collections.Generic;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Store.Notification;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Templates;

public partial class Index
{
    [Inject]
    private IState<NotificationRuleState> State { get; set; }
    
    private RadzenDataGrid<MessageTemplateDto> _grid;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(new FetchMessageTemplatesAction());
    }
    
    private void OnTemplateSelected(IList<MessageTemplateDto> template)
    {
        
    }
}
