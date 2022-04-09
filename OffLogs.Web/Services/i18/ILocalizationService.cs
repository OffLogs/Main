using System.Threading.Tasks;

namespace OffLogs.Web.Services.i18;

public interface ILocalizationService
{
    Task PreConfigureFromLocalStorageAsync();
    
    Task SetLocaleAsync(string locale);
}
