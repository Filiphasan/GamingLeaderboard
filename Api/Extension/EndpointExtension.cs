using Api.Common.Model;

namespace Api.Extension;

public static class EndpointExtension
{
    public static IResult ToHttpResult<T>(this Result<T> result) where T : class
    {
        return Results.Json(result, contentType: "application/json", statusCode: result.StatusCode);
    }
}