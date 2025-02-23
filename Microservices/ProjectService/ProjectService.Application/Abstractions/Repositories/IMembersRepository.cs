using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Repositories
{
    public interface IMembersRepository
    {
        public Task<(Guid, Guid)> AddAsync(Guid projectId, MemberModel model);
        public Task<(Guid, Guid)> DeleteAsync(Guid projectId, Guid userID);
        public Task<(Guid, Guid)> UpdateRoleAsync(Guid projectId, Guid userID, int roleId);
        public Task<List<MemberModel>> GetAllAsync(Guid projectId);
        
    }
}
