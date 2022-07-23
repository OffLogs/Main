using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace OffLogs.Business.Services.Redis;

public class RedisClient: IRedisClient
{
    private readonly ILogger<RedisClient> _logger;
    private readonly string _redisHost;

    private ConnectionMultiplexer _connection = null;
    protected IDatabase Db {
        get
        {
            if (_connection == null || !_connection.IsConnected)
            {
                _connection = ConnectionMultiplexer.Connect(_redisHost);
                _logger.LogDebug($"Connected to Redis server: {_redisHost}");
            }

            return _connection.GetDatabase();
        }
    }

    public RedisClient(IConfiguration configuration, ILogger<RedisClient> logger)
    {
        _logger = logger;
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
