using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using OffLogs.Business.Extensions;

namespace OffLogs.Web.Core.Helpers;

public static class EnumHelpers
{
    public static string GetDisplayName(Type enumType, object value)
    {
        if (enumType is null)
            throw new ArgumentNullException(nameof(enumType));

        return enumType.GetDisplayName(value);
    } 
}
