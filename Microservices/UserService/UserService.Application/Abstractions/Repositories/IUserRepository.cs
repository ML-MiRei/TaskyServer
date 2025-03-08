using UserService.Core.Models;

namespace UserService.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateAsync(Guid id, string email);
        Task<List<UserModel>> FindByNameAsync(string userName);
        Task<UserModel> GetByIdAsync(Guid userId);
        Task<List<UserModel>> GetByIdAsync(Guid[] userIds);
        Task<Guid> UpdateAsync(UserModel userModel);
    }
}