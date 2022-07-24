using Commands.Abstractions;
using OffLogs.Business.Orm.Entities;
using System;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Orm.Commands.Entities.Application
{
    public class ApplicationDeleteShareContext : ICommandContext
    {
        public ApplicationDeleteShareContext(ApplicationEntity application, UserEntity user)
        {
            Application = application ?? throw new ArgumentNullException(nameof(application));
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public ApplicationEntity Application { get; }
        public UserEntity User { get; }
    }
}