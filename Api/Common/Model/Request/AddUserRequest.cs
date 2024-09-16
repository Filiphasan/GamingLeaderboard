using Api.Feature.User.AddUser;

namespace Api.Common.Model.Request;

public class AddUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? DeviceId { get; set; }


    public AddUserCommand ToCommand() => new()
    {
        Username = Username!,
        Password = Password!,
        DeviceId = Guid.Parse(DeviceId!)
    };
}