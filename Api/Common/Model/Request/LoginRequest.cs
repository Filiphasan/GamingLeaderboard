using Api.Feature.Auth.Login;

namespace Api.Common.Model.Request;

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }


    public LoginCommand ToCommand() => new()
    {
        Username = Username!,
        Password = Password!
    };
}