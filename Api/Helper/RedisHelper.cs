using Api.Common.Model.Options;
using StackExchange.Redis;

namespace Api.Helper;

public static class RedisHelper
{
    public static IConnectionMultiplexer GetConnection(AppSettingRedis appSettingRedis)
    {
        var redisOptions = ConfigurationOptions.Parse($"{appSettingRedis.Host}:{appSettingRedis.Port}");
        redisOptions.Password = appSettingRedis.Password;
        redisOptions.AbortOnConnectFail = false;
        var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
        return multiplexer;
    }
}