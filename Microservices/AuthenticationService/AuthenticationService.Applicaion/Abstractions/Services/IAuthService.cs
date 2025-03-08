using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Common;

namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IAuthService
    {
        public Result<string?> Login(AuthDTO authData);
        public Result<string> Register(AuthDTO userData);

    }
}
