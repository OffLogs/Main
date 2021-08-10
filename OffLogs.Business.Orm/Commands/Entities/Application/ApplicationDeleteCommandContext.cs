using Commands.Abstractions;

namespace OffLogs.Business.Orm.Commands.Entities.Application
{
    public class ApplicationDeleteCommandContext : ICommandContext
    {
        public ApplicationDeleteCommandContext(long applicationId)
        {
            ApplicationId = applicationId;
        }

        public long ApplicationId { get; }
    }
}