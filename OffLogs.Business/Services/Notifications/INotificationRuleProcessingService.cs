using System.Threading.Tasks;
using Domain.Abstractions;

namespace OffLogs.Business.Services.Notifications;

public interface INotificationRuleProcessingService: IDomainService
{
    Task FindAndProcessWaitingRules();
}
