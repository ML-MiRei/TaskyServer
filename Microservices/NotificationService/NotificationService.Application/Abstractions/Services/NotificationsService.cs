using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Core.Models;

namespace NotificationService.Application.Abstractions.Services
{
    public class NotificationsService(INotificationsRepository notificationsRepository, INotificationSender notificationSender)
    {
        public async void Create(string[] users, NotificationModel notificationModel)
        {
            var notifications = await notificationsRepository.CreateAsync(users, notificationModel);
            notificationSender.SendNotifications(users, notifications.ToArray());
        }

        public async void Create(string user, NotificationModel notificationModel)
        {
            notificationModel.Id = await notificationsRepository.CreateAsync(user, notificationModel);
            notificationSender.SendNotification(user, notificationModel);
        }
    }
}
