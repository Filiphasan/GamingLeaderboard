using Api.Common.Model;
using Api.Data;
using Api.Data.Entities;
using Api.Service.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Feature.User.AddUser;

public class AddUserCommandHandler(
    LeaderboardContext context,
    IPasswordService passwordService
): IRequestHandler<AddUserCommand, Result<AddUserResponse>>
{
    public async Task<Result<AddUserResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var usernameExists = await context.Users.AnyAsync(x => x.Username == request.Username, cancellationToken);
        if (usernameExists)
        {
            return Result<AddUserResponse>.BadRequest(MessageConstant.UserMessage.UsernameAlreadyExists);
        }

        var hashedPassword = await passwordService.HashPasswordAsync(request.Password);
        var user = new Data.Entities.User
        {
            Username = request.Username,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow,
        };
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var userDevice = new UserDevice
        {
            UserId = user.Id,
            DeviceId = request.DeviceId,
            CreatedAt = DateTime.UtcNow
        };
        await context.UserDevices.AddAsync(userDevice, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result<AddUserResponse>.Success(new AddUserResponse());
    }
}