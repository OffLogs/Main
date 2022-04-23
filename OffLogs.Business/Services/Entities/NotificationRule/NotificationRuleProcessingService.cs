using System.Threading.Tasks;
using Commands.Abstractions;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.NotificationRule;

public class NotificationRuleProcessingService: INotificationRuleProcessingService
{
    private readonly IAsyncCommandBuilder _commandBuilder;
    private readonly IAsyncQueryBuilder _queryBuilder;

    public NotificationRuleProcessingService(
        IAsyncCommandBuilder commandBuilder,
        IAsyncQueryBuilder queryBuilder    
    )
    {
        _commandBuilder = commandBuilder;
        _queryBuilder = queryBuilder;
    }

    public Task FindAndProcessNextRule()
    {
        
        
        return Task.CompletedTask;
    }
}
