using System;
using System.Collections.Generic;
using System.Linq;
using OffLogs.Business.Extensions;
using OffLogs.Web.Shared.Ui.Form.CustomDropDown;

namespace OffLogs.Web.Core.Extensions;

public static class EnumExtensions
{
    public static ICollection<DropDownListItem> ToDropDownList(this Enum someEnum)
    {
        return Enum.GetValues(someEnum.GetType()).Cast<Enum>().Select(item => new DropDownListItem()
        {
            Id = item.ToString(),
            Label = item.GetDisplayName()
        }).ToList();
    }
}
