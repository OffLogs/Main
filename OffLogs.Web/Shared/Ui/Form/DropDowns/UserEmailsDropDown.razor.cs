using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Store.UserEmails;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class UserEmailsDropDown
{
    [Parameter]
    public ICollection<long> Value
    {
        get => _selectedIds.ToArray();
        set
        {
            if (value == null)
            {
                _selectedIds.Clear();
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
    public string Placeholder { get; set; } = CommonResources.SelectUserEmails;
    
    [Parameter]
    public string Class { get; set; }
    
    [Inject]
    private IState<UserEmailsState> State { get; set; }
    
    private ICollection<UserEmailDto> _selectedItems;

    private ICollection<long> _selectedIds = new List<long>();

    IEnumerable<string> multipleValues;

    private IEnumerable<UserEmailDto> _list2 = new List<UserEmailDto>()
    {
        new UserEmailDto() { Id = 1, Email = "test"}
    };
    
    private ICollection<UserEmailDto> _list
    {
        get => State.Value.List.Where(item => item.IsVerified).ToList();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(new FetchListAction(true));
    }

    private void OnValueChanged(ICollection<long>? selectedIds)
    {
        Debug.Log(selectedIds);
        // _selectedItems = _list.Where(item => selectedIds.Contains(item.Id)).ToList();
        // Debug.Log(selectedIds);
        // SelectedItemChanged.InvokeAsync(_selectedItems.ToArray());
        // ValueChanged.InvokeAsync(selectedIds.ToArray());
    }
    
    private void Callback(object value)
    {
        var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
        Debug.Log(str);
    }
}
