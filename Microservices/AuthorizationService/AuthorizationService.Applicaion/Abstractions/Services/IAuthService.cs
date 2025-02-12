using AuthorizationService.Applicaion.DTO;
using AuthorizationService.Core.Common;
using AuthorizationService.Core.Models;

namespace AuthorizationService.Applicaion.Abstractions.Services
{
    public interface IAuthService
    {
        public Result<UserModel> Login(AuthDTO authData);
        public Result<Guid> Register(UserDTO userData);

    }
}
