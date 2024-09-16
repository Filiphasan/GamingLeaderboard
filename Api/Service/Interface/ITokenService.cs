using Api.Common.Model.Dto;

namespace Api.Service.Interface;

public interface ITokenService
{
    Task<(string, int)> CreateTokenAsync(TokenUserModel model);
}