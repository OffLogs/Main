using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Security
{
    public class AccessPolicyService: IAccessPolicyService
    {
        private readonly IAsyncQueryBuilder _queryBuilder;

        public AccessPolicyService(IAsyncQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
        }

        #region Write
        public async Task<bool> HasWriteAccessAsync<TEntity>(long entityId, long userId) where TEntity : class, IEntity, new()
        {
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            var entity = await _queryBuilder.FindByIdAsync<TEntity>(entityId);
            return await HasWriteAccessAsync(entity, user);
        }

        public async Task<bool> HasWriteAccessAsync(IEntity entity, UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is ApplicationEntity)
            {
                return HasAccessApplication(entity as ApplicationEntity, user, false);
            }
            return await Task.FromResult(false);
        }
        #endregion

        #region Read
        public async Task<bool> HasReadAccessAsync<TEntity>(long entityId, long userId) where TEntity : class, IEntity, new()
        {
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            var entity = await _queryBuilder.FindByIdAsync<TEntity>(entityId);
            return await HasReadAccessAsync(entity, user);
        }

        public async Task<bool> HasReadAccessAsync(IEntity entity, UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is ApplicationEntity)
            {
                return HasAccessApplication(entity as ApplicationEntity, user, true);
            }
            return await Task.FromResult(false);
        }
        #endregion

        #region Entity handlers
        private bool HasAccessApplication(ApplicationEntity entity, UserEntity user, bool IsRead)
        {
            bool isHas = entity.User.Id == user.Id;
            if (IsRead && !isHas)
            {
                isHas = entity.SharedForUsers.Any(u => u.Id == user.Id);
            }
            return isHas;
        }
        #endregion
    }
}
