using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Abstractions.Repositories
{
    public interface IAuthDataRepository
    {
        public Task<AuthDataModel> GetByEmail(string email);
        public Task<Guid> Create(AuthDataModel authData);
        public Task<Guid?> UpdateEmailAsync(Guid userId, string email);
        public Task<Guid?> UpdatePasswordAsync(Guid userId, string password);
    }
}
