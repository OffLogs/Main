using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace OffLogs.Business.Services.Redis;

public class RedisClient: IRedisClient, IDisposable
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
                
                _connection.ConnectionFailed += OnConnectionFailed;
                _connection.ErrorMessage += OnErrorMessage;
                _connection.InternalError += OnInternalError;
            }

            return _connection.GetDatabase();
        }
    }

    public RedisClient(IConfiguration configuration, ILogger<RedisClient> logger)
    {
        _logger = logger;
        _redisHost = configuration.GetValue<string>("Redis:Sever");
    }

    #region Event Actions
    
    private void OnErrorMessage(object sender, RedisErrorEventArgs e)
    {
        _logger.LogError($"Redis error: {e.Message}");
    }

    private void OnConnectionFailed(object sender, ConnectionFailedEventArgs e)
    {
        _logger.LogError(e.Exception, $"Redis connection failed: {e.Exception?.Message}");
    }
    
    private void OnInternalError(object sender, InternalErrorEventArgs e)
    {
        _logger.LogError(e.Exception, $"Redis internal error: {e.Exception.Message}");
    }
    
    #endregion
    
    public void Dispose()
    {
        _connection?.Dispose();
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
