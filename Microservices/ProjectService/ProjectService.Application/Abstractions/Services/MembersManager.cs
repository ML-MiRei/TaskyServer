using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Enums;
using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Services
{
    public class MembersManager(IMembersRepository membersRepository, IProjectsRepository projectsRepository, INotificationService notificationService)
    {
        public async Task<string> AddMemberAsync(MemberModel memberModel)
        {
            memberModel.Role = new RoleModel((int)MemberRoles.Observer, MemberRoles.Observer.ToString());

            var user = await membersRepository.AddAsync(memberModel);

            Task.Run(async () =>
            {
                var project = await projectsRepository.GetByIdAsync(memberModel.Id);

                var text = $"Вы были добавлены в проект \"{project.Title}\"";
                var title = "Новый проект";

                var message = new Dtos.MessageModel([user], text, title, project.Id);

                notificationService.SendNotification(message);
            });

            return user;
        }

        public async Task<string> ChangeMemberRoleAsync(MemberModel memberModel)
        {
            var user = await membersRepository.UpdateRoleAsync(memberModel);

            Task.Run(async () =>
            {
                var project = await projectsRepository.GetByIdAsync(memberModel.Id);

                var text = $"Ваша роль в проекте \"{project.Title}\" была изменена";

                var message = new Dtos.MessageModel([user], text, project.Title, project.Id);

                notificationService.SendNotification(message);
            });

            return user;
        }

        public async Task<string> DeleteMemberAsync(MemberModel memberModel)
        {
            var user = await membersRepository.DeleteAsync(memberModel);

            Task.Run(async () =>
            {
                var project = await projectsRepository.GetByIdAsync(memberModel.Id);

                var text = $"Вы были удалены из проекта \"{project.Title}\"";

                var message = new Dtos.MessageModel([user], text, project.Title);

                notificationService.SendNotification(message);
            });

            return user;
        }
    }
}
