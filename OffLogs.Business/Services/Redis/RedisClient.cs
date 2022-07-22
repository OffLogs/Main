using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace OffLogs.Business.Services.Redis;

public class RedisClient: IRedisClient
{
    private readonly string _redisHost;

    private ConnectionMultiplexer _connection = null;
    protected IDatabase Db {
        get
        {
            if (_connection == null || !_connection.IsConnected)
            {
                _connection = ConnectionMultiplexer.Connect(_redisHost);
            }

            return _connection.GetDatabase();
        }
    }

    public RedisClient(IConfiguration configuration)
    {
        _redisHost = configuration.GetValue<string>("Redis:Sever");
    }

    #region Common
    
    public async Task SetString(string key, string value)
    {
        await Db.StringSetAsync(key, value);
    }
    
    public async Task<string> GetString(string key)
    {
        return await Db.StringGetAsync(key);
    }
    
    #endregion
}
