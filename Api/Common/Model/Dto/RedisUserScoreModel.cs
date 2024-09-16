namespace Api.Common.Model.Dto;

public class RedisUserScoreModel
{
    public required string Username { get; set; }
    public required int Score { get; set; }
}