using Api.Feature.UserScore.AddUserScore;

namespace Api.Common.Model.Request;

public class AddUserScoreRequest
{
    public int Score { get; set; }

    public AddUserScoreCommand ToCommand(int userId) => new AddUserScoreCommand
    {
        Score = Score,
        UserId = userId
    };
}