using Api.Common.Model;
using MediatR;

namespace Api.Feature.UserScore.AddUserScore;

public class AddUserScoreCommand : IRequest<Result<AddUserScoreResponse>>
{
    public int UserId { get; set; }
    public int Score { get; set; }
}

public class AddUserScoreResponse
{
    
}