using Api.Common.Model.Request;
using Api.Extension;
using Carter;
using MediatR;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Api.Endpoint;

public class UserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .AddFluentValidationAutoValidation()
            .WithTags("User Endpoints");
        
        group.MapPost("/", AddUserAsync);
    }

    private static async Task<IResult> AddUserAsync(AddUserRequest request, ISender sender)
    {
        var result = await sender.Send(request.ToCommand());
        return result.ToHttpResult();
    }
}