using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Core.Models;
using AuthenticationService.Infrastructure.Implementations.Services.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Infrastructure.Services
{
    public class JwtProvider(IOptions<JwtOptions> options): IJwtProvider
    {
        public string GenerateToken(AuthDataModel authDataModel)
        {
            var values = options.Value;
            Claim[] claims = [
                new Claim("userId", authDataModel.UserId.ToString()),
                new Claim("email", authDataModel.Email)
                ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(values.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(values.ExpiresHours)
                );

            var tockenValue = new JwtSecurityTokenHandler().WriteToken( token );

            return tockenValue;
        }
    }
}
