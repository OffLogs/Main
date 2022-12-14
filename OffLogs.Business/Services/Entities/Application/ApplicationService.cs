using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using Notification.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Exceptions.Common;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Commands.Entities.Application;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using OffLogs.Business.Services.Monetization;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.Application
{
    public class ApplicationService: IApplicationService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtApplicationService _jwtService;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly IRestrictionValidationService _restrictionValidationService;

        public ApplicationService(
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder asyncQueryBuilder,
            IJwtApplicationService jwtService,
            IKafkaProducerService kafkaProducer,
            IRestrictionValidationService restrictionValidationService
        )
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = asyncQueryBuilder;
            _jwtService = jwtService;
            _kafkaProducer = kafkaProducer;
            _restrictionValidationService = restrictionValidationService;
        }

        public async Task<ApplicationEntity> CreateNewApplication(UserEntity user,  string name)
        {
            _restrictionValidationService.CheckApplicationsAddingAvailable(user);
            
            var userEncryptor = AsymmetricEncryptor.FromPublicKeyBytes(user.PublicKey);
            
            var application = new ApplicationEntity(name);
            user.AddApplication(application);
            
            // Application keys encryption
            var applicationEncryptor = AsymmetricEncryptor.GenerateKeyPair();
            application.PublicKey = applicationEncryptor.GetPublicKeyBytes();
            application.EncryptedPrivateKey = userEncryptor.EncryptData(
                applicationEncryptor.GetPrivateKeyBytes()
            );
            
            await _commandBuilder.SaveAsync(application);
            application.ApiToken = _jwtService.BuildJwt(application);
            
            await _commandBuilder.SaveAsync(application);
            return application;
        }

        public async Task<ApplicationEntity> UpdateApplication(long applicationId, string name)
        {
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(applicationId);
            if (application == null)
            {
                throw new ItemNotFoundException(nameof(ApplicationEntity));
            }
            application.Name = name;
            application.UpdateTime = System.DateTime.UtcNow;
            await _commandBuilder.SaveAsync(application);
            return application;
        }

        /// <summary>
        /// Should add rights for the other user for this app
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ShareForUser(ApplicationEntity application, UserEntity user)
        {
            if (application.User.Id == user.Id)
            {
                throw new PermissionException("This is application owner");
            }
            if (application.SharedForUsers.Any(u => u.Id == user.Id))
            {
                throw new PermissionException("This user already has access right for this application");
            }

            if (application.SharedForUsers.Count >= GlobalConstants.ApplicationMaxShares)
            {
                throw new PermissionException($"You can not share application more than {GlobalConstants.ApplicationMaxShares} users");
            }
            application.SharedForUsers.Add(user);
            await _commandBuilder.SaveAsync(application);
        }

        /// <summary>
        /// Should remove access rights of the user for this app
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task RemoveShareForUser(ApplicationEntity application, UserEntity user)
        {
            if (application.User.Id == user.Id)
            {
                throw new PermissionException("This is application owner");
            }
            if (application.SharedForUsers.Any(u => u.Id == user.Id))
            {
                application.SharedForUsers.Remove(user);
                await _commandBuilder.SaveAsync(application);
                return;
            }
            throw new PermissionException("This user does not have access right for this application");
        }

        public async Task Delete(long applicationId)
        {
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(applicationId);
            if (application == null)
            {
                throw new ItemNotFoundException(nameof(ApplicationEntity));
            }

            var userToNotify = application.SharedForUsers.ToList();
            // add owner
            userToNotify.Add(application.User);

            await _commandBuilder.ExecuteAsync(new ApplicationDeleteCommandContext(applicationId));

            foreach (var user in userToNotify)
            {
                await _kafkaProducer.ProduceNotificationMessageAsync(new ApplicationDeletedNotificationContext(
                    user.Email,
                    application.Name
                ));
            }
        }
    }
}
