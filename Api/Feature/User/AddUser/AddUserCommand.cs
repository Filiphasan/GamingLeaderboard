using Api.Common.Model;
using MediatR;

namespace Api.Feature.User.AddUser;

public class AddUserCommand : IRequest<Result<AddUserResponse>>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required Guid DeviceId { get; set; }
}

public class AddUserResponse
{
    
}