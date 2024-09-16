using System.Text.Json;
using Api.Common.Model.Dto;
using Api.Common.Model.Options;
using Api.Service.Interface;
using StackExchange.Redis;

namespace Api.Service.Implementation;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, AppSetting appSetting)
    {
        ArgumentNullException.ThrowIfNull(connectionMultiplexer);
        _database = connectionMultiplexer.GetDatabase(appSetting.Redis.Database);
    }
    
    public async Task PingAsync()
    {
        await _database.PingAsync();
    }

    public async Task SetAsync<TModel>(string key, TModel value, TimeSpan expiration) where TModel : class, new()
    {
        var redisValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, redisValue, expiration);
    }

    public async Task<TModel?> GetAsync<TModel>(string key) where TModel : class, new()
    {
        var redisValue = await _database.StringGetAsync(key);
        return redisValue.HasValue
            ? JsonSerializer.Deserialize<TModel>(redisValue.ToString())
            : null;
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task AddUserScoreAsync(string key, string userName, int score)
    {
        await _database.SortedSetIncrementAsync(key, userName, score);
    }

    public async Task<RedisUserScoreModel[]> GetUserScoresAsync(string key, long start, long stop)
    {
        var userScores = await _database.SortedSetRangeByRankWithScoresAsync(key, start, stop - 1, Order.Descending);
        return userScores.Select(x => new RedisUserScoreModel
        {
            Username = x.Element.ToString(),
            Score = (int)x.Score
        }).ToArray();
    }
}