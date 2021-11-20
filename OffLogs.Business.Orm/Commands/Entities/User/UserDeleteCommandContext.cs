using Commands.Abstractions;

namespace OffLogs.Business.Orm.Commands.Entities.User
{
    public class UserDeleteCommandContext : ICommandContext
    {
        public string UserName { get; }
        public string Email { get; }

        public UserDeleteCommandContext(string userName, string email = null)
        {
            UserName = userName;
            Email = email;
        }
    }
}
