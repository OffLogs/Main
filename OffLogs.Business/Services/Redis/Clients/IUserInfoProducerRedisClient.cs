using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Common.Constants.Monetization;

namespace OffLogs.Business.Services.Redis.Clients;

public interface IUserInfoProducerRedisClient: IDomainService
{
    Task SeedUsersPackages(CancellationToken cancellationToken = default);
}
