using System.IdentityModel.Tokens.Jwt;
using Api.Common.Model.Request;
using Api.Extension;
using Api.Feature.UserScore.GetTopUserScores;
using Carter;
using MediatR;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Api.Endpoint;

public class UserScoreEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/user-scores")
            .AddFluentValidationAutoValidation()
            .WithTags("User Score Endpoints")
            .RequireAuthorization();

        group.MapPost("/", AddUserScoreAsync);
        group.MapGet("/top", GetTopUserScoresAsync);
    }

    private static async Task<IResult> AddUserScoreAsync(AddUserScoreRequest request, ISender sender, IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var result = await sender.Send(request.ToCommand(int.Parse(userId!)));
        return result.ToHttpResult();
    }

    private static async Task<IResult> GetTopUserScoresAsync(ISender sender)
    {
        var result = await sender.Send(new GetTopUserScoresQuery());
        return result.ToHttpResult();
    }
}