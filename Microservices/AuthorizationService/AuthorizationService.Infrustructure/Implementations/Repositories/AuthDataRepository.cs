using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Core.Models;

namespace AuthenticationService.Infrastructure.Implementations.Repositories
{
    public class AuthDataRepository : IAuthDataRepository
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
