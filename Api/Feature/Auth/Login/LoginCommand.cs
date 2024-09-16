using Api.Common.Model;
using MediatR;

namespace Api.Feature.Auth.Login;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
}