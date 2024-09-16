using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Common.Model.Dto;
using Api.Common.Model.Options;
using Api.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Api.Service.Implementation;

public class TokenService(AppSetting appSetting) : ITokenService
{
    public Task<(string, int)> CreateTokenAsync(TokenUserModel model)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.Jwt.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claimList = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, model.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, model.Username),
        };
        var jwtToken = new JwtSecurityToken(claims: claimList, expires: DateTime.Now.AddMinutes(appSetting.Jwt.ExpiresInMinutes), signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return Task.FromResult((token, appSetting.Jwt.ExpiresInMinutes));
    }
}