using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using ProjectService.Infrastructure.Database;
using ProjectService.Infrastructure.Database.Entities;

namespace ProjectService.Infrastructure.Implementations.Repositories
{
    public class MembersRepository(ProjectsDbContext context) : IMembersRepository
    {
        public async Task<string> AddAsync(MemberModel model)
        {
            var newMember = new MemberEntity
            {
                UserId = model.Id,
                RoleId = model.Role.Id,
                ProjectId = model.ProjectId,
            };

            var memdr = await context.Members.AddAsync(newMember);
            await context.SaveChangesAsync();

            return newMember.UserId;
        }

        public async Task<string> DeleteAsync(MemberModel memberModel)
        {
            var member = await context.Members.FirstAsync(p => p.UserId == memberModel.Id && p.ProjectId == memberModel.ProjectId);

            context.Members.Remove(member);
            await context.SaveChangesAsync();

            return member.UserId;
        }

        public Task<List<MemberModel>> GetAllAsync(string projectId)
        {
            var members = context.Members.AsNoTracking()
                .Where(m => m.ProjectId == projectId)
                .Select(m => new MemberModel(m.UserId, m.ProjectId, new RoleModel(m.RoleId, m.Role.Name)));

            return Task.FromResult(members.ToList());
        }

        public async Task<RoleModel> GetRoleAsync(MemberModel memberModel)
        {
            var member = await context.Members.AsNoTracking()
                .Include(m => m.Role)
                .FirstAsync(m => m.ProjectId == memberModel.ProjectId && m.UserId == memberModel.Id);

            return new RoleModel(member.RoleId, member.Role.Name);
        }


        public async Task<string> UpdateRoleAsync(MemberModel memberModel)
        {
            var member = context.Members
               .AsNoTracking()
               .FirstOrDefault(p => p.UserId == memberModel.Id && p.ProjectId == memberModel.ProjectId);

            member.RoleId = memberModel.Role.Id;

            context.Update(member);
            await context.SaveChangesAsync();

            return member.UserId;
        }

       
    }
}
