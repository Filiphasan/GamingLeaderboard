using Api.Common.Model;
using Api.Data;
using Api.Service.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Feature.UserScore.AddUserScore;

public class AddUserScoreCommandHandler(
    LeaderboardContext context,
    IRedisCacheService redisCacheService
) : IRequestHandler<AddUserScoreCommand, Result<AddUserScoreResponse>>
{
    public async Task<Result<AddUserScoreResponse>> Handle(AddUserScoreCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstAsync(x => x.Id == request.UserId, cancellationToken);
        await Task.WhenAll(AddUserScoreToDb(request, user, cancellationToken), AddUserScoreToRedis(request, user));
        return Result<AddUserScoreResponse>.Success(new AddUserScoreResponse());
    }

    private async Task AddUserScoreToDb(AddUserScoreCommand request, Data.Entities.User user, CancellationToken cancellationToken)
    {
        var userScore = new Data.Entities.UserScore
        {
            UserId = user.Id,
            Score = request.Score,
            CreatedAt = DateTime.Now
        };
        await context.UserScores.AddAsync(userScore, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task AddUserScoreToRedis(AddUserScoreCommand request, Data.Entities.User user)
    {
        await redisCacheService.AddUserScoreAsync(RedisConstant.LeaderboardKey, user.Username, request.Score);
    }
}