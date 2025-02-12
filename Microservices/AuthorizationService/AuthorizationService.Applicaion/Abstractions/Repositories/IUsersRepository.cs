using AuthorizationService.Core.Models;

namespace AuthorizationService.Applicaion.Abstractions.Repositories
{
    public interface IUsersRepository
    {
        public UserModel? GetByEmail(string email);
        public Guid Create(UserModel user);
        public Guid? Update(UserModel user);
        public Guid? Delete(Guid? id);
    }
}
