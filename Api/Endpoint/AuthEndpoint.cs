using Api.Common.Model.Request;
using Api.Extension;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Api.Endpoint;

public class AuthEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auths")
            .AddFluentValidationAutoValidation()
            .WithTags("Auth Endpoints");

        group.MapPost("/login", LoginAsync);
    }

    private static async Task<IResult> LoginAsync([FromBody] LoginRequest request, ISender sender)
    {
        var result = await sender.Send(request.ToCommand());
        return result.ToHttpResult();
    }
}