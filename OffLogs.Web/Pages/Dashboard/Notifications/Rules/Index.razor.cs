using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Store.Notification;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Rules;

public partial class Index
{
    [Inject] 
    private IState<NotificationRuleState> _state { get; set; }
    
    private RadzenDataGrid<NotificationRuleDto> _grid;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        Dispatcher.Dispatch(new FetchNotificationRulesAction());
    }
}
