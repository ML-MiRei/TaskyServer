using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Abstractions.Repositories
{
    public interface IAuthDataRepository
    {
        public Task<AuthDataModel> GetByEmail(string email);
        public Task<string> Create(AuthDataModel authData);
        public Task<string?> UpdateEmailAsync(string userId, string email);
        public Task<string?> UpdatePasswordAsync(string userId, string password);
        public Task SetIsVerify(string userId);
    }
}
