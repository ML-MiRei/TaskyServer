using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Core.Models;

namespace NotificationService.Application.Abstractions.Services
{
    public class NotificationsService(INotificationsRepository notificationsRepository)
    {
        public async void Create(NotificationModel notificationModel)
        {
            await notificationsRepository.CreateAsync(notificationModel);
        }
    }
}
