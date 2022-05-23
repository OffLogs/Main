using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class LogLevelDropDown
{
    [Parameter]
    public LogLevel? Value
    {
        get => _value;
        set => _value = value ?? default;
    }
    
    [Parameter]
    public EventCallback<LogLevel?> ValueChanged { get; set; }
    
    private List<LogLevel> _list;
    private LogLevel _value;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _list = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();
    }
}
