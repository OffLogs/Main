using System.Threading.Tasks;
using Domain.Abstractions;

namespace OffLogs.Business.Services.Redis;

public interface IRedisClient: IDomainService
{
    #region Common

    Task SetString(string key, string value);
    Task<string> GetString(string key);

    #endregion
}
