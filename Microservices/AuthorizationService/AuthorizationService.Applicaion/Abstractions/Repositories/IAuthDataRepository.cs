using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Abstractions.Repositories
{
    public interface IAuthDataRepository
    {
        public UserModel? GetByEmail(string email);
        public Guid Create(UserModel user);
        public Guid? Update(UserModel user);
        public Guid? Delete(Guid? id);
    }
}
