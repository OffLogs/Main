using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.UserEmails;

namespace OffLogs.Web.Shared.Ui.Form.Checkboxes;

public partial class UserEmailsCheckbox
{
    [Parameter]
    public IEnumerable<long> Value
    {
        get => _selectedIds.ToArray();
        set
        {
            if (value == null)
            {
                _selectedIds = new long []{};
                return;
            }

            _selectedIds = value;
        }
    }
    
    [Parameter]
    public EventCallback<long[]> ValueChanged { get; set; }

    [Parameter]
    public EventCallback<UserEmailDto[]> SelectedItemChanged { get; set; }
    
    [Parameter]
    public string Label { get; set; } = CommonResources.SelectUserEmails;
    
    [Parameter]
    public string Class { get; set; }
    
    [Inject]
    private IState<UserEmailsState> State { get; set; }

    private ICollection<UserEmailDto> _selectedItems;

    private IEnumerable<long> _selectedIds = new long[] { };

    private ICollection<UserEmailDto> _list = new List<UserEmailDto>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        State.StateChanged += (sender, args) =>
        {
            _list.Clear();
            var newList = State.Value.List
                .Where(item => item.IsVerified)
                .ToArray();
            foreach (var item in newList)
            {
                _list.Add(item);
            }
            StateHasChanged();
        };
        Dispatcher.Dispatch(new FetchListAction(true));
    }
    
    private void OnCheckboxChecked(IEnumerable<long> selectedIds)
    {
        _selectedItems = _list.Where(item => selectedIds.Contains(item.Id)).ToList();
        SelectedItemChanged.InvokeAsync(_selectedItems.ToArray());
        ValueChanged.InvokeAsync(selectedIds.ToArray());
    }
}
