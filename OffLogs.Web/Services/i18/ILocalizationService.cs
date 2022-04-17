using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OffLogs.Web.Services.i18;

public interface ILocalizationService
{
    Task SetUpLocaleAsync();
    
    Task SetLocaleAsync(string locale);

    CultureInfo GetLocale();
    
    ICollection<CultureInfo> GetAwailableLocales();
    
    string GetLocalePostfix();
}
