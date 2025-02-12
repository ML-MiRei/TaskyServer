using AuthorizationService.Applicaion.Abstractions.Repositories;
using AuthorizationService.Core.Models;

namespace AuthorizationService.Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        public Guid Create(UserModel user)
        {
            return Guid.NewGuid();
        }

        public Guid? Delete(Guid? id)
        {
            throw new NotImplementedException();
        }

        public UserModel? GetByEmail(string email)
        {
            return UserModel.Create(Guid.NewGuid(), "Name", "79089089999", email, "has3Hskksh").Value;
        }

        public Guid? Update(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
