using System;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Notification;
using Radzen;

namespace OffLogs.Web.Pages.Dashboard.Notifications.Rules.Parts;

public partial class EditRuleForm
{
    [Inject] private IState<NotificationRuleState> _state { get; set; }

    [Inject] private IState<ApplicationsListState> _applicationState { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter] public long Id { get; set; }

    public SetRuleRequest Model = new() {Type = NotificationType.Email.ToString()};
    private bool _isLoading = false;

    private LogicOperatorType _logicOperatorType
    {
        get
        {
            if (string.IsNullOrEmpty(Model.LogicOperator))
            {
                return default;
            }

            return Enum.Parse<LogicOperatorType>(Model.LogicOperator);
        }
        set => Model.LogicOperator = value.ToString();
    }

    private long _applicationId
    {
        get => Model.ApplicationId ?? default;
        set => Model.ApplicationId = value == default ? null : value;
    }

    private bool _isNew => Id == 0;
    private bool _canAddCondition => Model.Conditions.Count < 10;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Dispatcher.Dispatch(new FetchListPageAction());
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (Model.Id != Id)
        {
            Model.Id = Id;
            if (_isNew)
            {
                Model.Reset();
            }
            else
            {
                var foundItem = _state.Value.Rules.FirstOrDefault(
                    item => item.Id == Id
                );
                Model.Fill(foundItem);
            }
        }
    }

    private async Task HandleSubmit()
    {
        _isLoading = true;
        try
        {
            var item = await ApiService.NotificationRuleSet(Model);
            DialogService.Close();
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = _isNew ? NotificationResources.Rules_Added : NotificationResources.Rules_Saved
            });
            Dispatcher.Dispatch(new SetNotificationRuleAction(item));
        }
        catch (Exception e)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = e.Message
            });
        }
        finally
        {
            _isLoading = false;
        }

        StateHasChanged();
    }

    private void OnAddCondition()
    {
        Model.Conditions.Add(new SetConditionRequest());
    }
    
    private void OnDeleteCondition(SetConditionRequest condition)
    {
        Model.Conditions.Remove(condition);
    }
}
