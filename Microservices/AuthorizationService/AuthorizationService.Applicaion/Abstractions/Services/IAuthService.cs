using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Common;
using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Abstractions.Services
{
    public interface IAuthService
    {
        public Result<UserModel> Login(AuthDTO authData);
        public Result<Guid> Register(UserDTO userData);

    }
}
