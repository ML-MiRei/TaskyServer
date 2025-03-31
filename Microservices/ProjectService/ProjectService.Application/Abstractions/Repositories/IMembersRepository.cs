using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Repositories
{
    public interface IMembersRepository
    {
        Task<string> AddAsync(MemberModel model);
        Task<string> DeleteAsync(MemberModel memberModel);
        Task<List<MemberModel>> GetAllAsync(string projectId);
        Task<string> UpdateRoleAsync(MemberModel memberModel);
        Task<RoleModel> GetRoleAsync(MemberModel memberModel);

    }
}
