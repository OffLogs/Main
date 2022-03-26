using System.Threading.Tasks;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Services.Web.ReCaptcha;

public interface IReCaptchaService: IDomainService
{
    Task<bool> ValidateAsync(string token);
}
