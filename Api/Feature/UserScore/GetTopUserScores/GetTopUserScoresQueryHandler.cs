using Api.Common.Model;
using Api.Data;
using Api.Service.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Feature.UserScore.GetTopUserScores;

public class GetTopUserScoresQueryHandler(
    IRedisCacheService redisCacheService,
    LeaderboardContext context
) : IRequestHandler<GetTopUserScoresQuery, Result<IList<GetTopUserScoresResponse>>>
{
    public async Task<Result<IList<GetTopUserScoresResponse>>> Handle(GetTopUserScoresQuery request, CancellationToken cancellationToken)
    {
        var redisList = await redisCacheService.GetUserScoresAsync(RedisConstant.LeaderboardKey, 0, RedisConstant.TopUserScoresCount);
        if (redisList.Length == 0)
        {
            await CheckUserScoreOnDbAndLoadRedis(cancellationToken);
        }

        var list = redisList
            .Select(x => new GetTopUserScoresResponse
            {
                Username = x.Username,
                Score = x.Score
            }).ToList();
        return Result<IList<GetTopUserScoresResponse>>.Success(list);
    }

    private async Task CheckUserScoreOnDbAndLoadRedis(CancellationToken cancellationToken)
    {
        var userScoresExist = await context.UserScores.AnyAsync(cancellationToken);
        if (!userScoresExist)
        {
            return;
        }

        var userScores = await context.UserScores
            .GroupBy(x => new { x.UserId })
            .Select(x => new
            {
                x.Key.UserId,
                Score = x.Max(y => y.Score)
            })
            .OrderByDescending(x => x.Score)
            .Take(RedisConstant.TopUserScoresCount)
            .ToListAsync(cancellationToken);
        var userNames = await context.Users
            .Where(x => userScores.Select(y => y.UserId).Contains(x.Id))
            .Select(x => new
            {
                x.Id,
                x.Username
            })
            .ToListAsync(cancellationToken);
        foreach (var userScore in userScores)
        {
            await redisCacheService.AddUserScoreAsync(RedisConstant.LeaderboardKey, userNames.Find(x => x.Id == userScore.UserId)!.Username, userScore.Score);
        }
    }
}