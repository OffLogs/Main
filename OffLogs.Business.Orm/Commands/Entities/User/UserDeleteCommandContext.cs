using Commands.Abstractions;

namespace OffLogs.Business.Orm.Commands.Entities.User
{
    public class UserDeleteCommandContext : ICommandContext
    {
        public string UserName { get; }

        public UserDeleteCommandContext(string userName)
        {
            UserName = userName;
        }
    }
}