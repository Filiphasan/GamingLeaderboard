using Api.Common.Model;
using MediatR;

namespace Api.Feature.UserScore.GetTopUserScores;

public class GetTopUserScoresQuery : IRequest<Result<IList<GetTopUserScoresResponse>>>
{
}

public class GetTopUserScoresResponse
{
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; }
}