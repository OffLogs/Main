using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;

namespace OffLogs.Business.Services.Notifications;

public interface INotificationRuleProcessingService: IDomainService
{
    Task FindAndProcessWaitingRules(CancellationToken cancellationToken = default);
}
