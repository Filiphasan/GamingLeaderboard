using Api.Common.Model.Dto;

namespace Api.Service.Interface;

public interface IRedisCacheService
{
    Task PingAsync();
    Task SetAsync<TModel>(string key, TModel value, TimeSpan expiration) where TModel : class, new();
    Task<TModel?> GetAsync<TModel>(string key) where TModel : class, new();
    Task RemoveAsync(string key);
    Task AddUserScoreAsync(string key, string userName, int score);
    Task<RedisUserScoreModel[]> GetUserScoresAsync(string key, long start, long stop);
}