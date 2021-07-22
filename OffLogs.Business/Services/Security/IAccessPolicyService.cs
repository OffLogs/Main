using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Security
{
    public interface IAccessPolicyService : IDomainService
    {
        Task<bool> HasWriteAccessAsync(IEntity entity, UserEntity user);
        Task<bool> HasWriteAccessAsync<TEntity>(long entityId, long userId) where TEntity : class, IEntity, new();
        Task<bool> HasReadAccessAsync(IEntity entity, UserEntity user);
        Task<bool> HasReadAccessAsync<TEntity>(long entityId, long userId) where TEntity : class, IEntity, new();
    }
}
