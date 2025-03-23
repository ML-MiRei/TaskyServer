using UserService.Core.Models;

namespace UserService.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateAsync(string id, string email);
        Task<List<UserModel>> FindByNameAsync(string userName);
        Task<UserModel> GetByIdAsync(string userId);
        Task<List<UserModel>> GetByIdAsync(string[] userIds);
        Task<string> UpdateAsync(UserModel userModel);
    }
}