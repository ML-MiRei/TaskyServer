using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;
using System.Data.Entity;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class MembersRepository(ProjectsDbContext context) : IMembersRepository
    {
        public async Task<(Guid, Guid)> AddAsync(Guid projectId, MemberModel model)
        {
            var newMember = new MemberEntity
            {
                UserId = model.Id,
                RoleId = model.RoleID,
                ProjectId = projectId
            };

            var memdr = await context.Members.AddAsync(newMember);
            await context.SaveChangesAsync();

            return (projectId, newMember.UserId);
        }

        public async Task<(Guid, Guid)> DeleteAsync(Guid projectId, Guid userID)
        {
            var member = context.Members.FirstOrDefault(p => p.UserId == userID);
            context.Members.Remove(member);
            await context.SaveChangesAsync();

            return (projectId, member.UserId);
        }

        public Task<List<MemberModel>> GetAllAsync(Guid projectId)
        {
            var members = context.Members
                .AsNoTracking()
                .Select(m => new MemberModel(m.UserId, m.RoleId, null, null));

            return Task.FromResult(members.ToList());
        }


        public async Task<(Guid, Guid)> UpdateRoleAsync(Guid projectId, Guid userID, int roleId)
        {
            var member = context.Members
               .AsNoTracking()
               .FirstOrDefault(p => p.UserId == userID && p.ProjectId == projectId);

            member.RoleId = roleId;

            context.Update(member);
            await context.SaveChangesAsync();

            return (projectId, member.UserId);
        }
    }
}
