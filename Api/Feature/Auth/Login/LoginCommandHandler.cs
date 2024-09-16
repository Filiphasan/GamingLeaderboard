using Api.Common.Model;
using Api.Common.Model.Dto;
using Api.Data;
using Api.Service.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Feature.Auth.Login;

public sealed class LoginCommandHandler(
    LeaderboardContext context,
    IPasswordService passwordService,
    ITokenService tokenService
) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);
        if (user is null)
        {
            return Result<LoginResponse>.BadRequest(MessageConstant.UserMessage.UsernameOrPasswordIsNotCorrect);
        }

        if (!await passwordService.VerifyPasswordAsync(request.Password, user.Password))
        {
            return Result<LoginResponse>.BadRequest(MessageConstant.UserMessage.UsernameOrPasswordIsNotCorrect);
        }

        var (accessToken, expiresIn) = await tokenService.CreateTokenAsync(new TokenUserModel { Id = user.Id, Username = user.Username });

        return Result<LoginResponse>.Success(new LoginResponse
        {
            AccessToken = accessToken,
            ExpiresIn = expiresIn
        });
    }
}