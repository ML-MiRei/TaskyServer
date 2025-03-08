using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IJwtProvider
    {
        public string GenerateToken(AuthDataModel authDataModel);
    }
}
