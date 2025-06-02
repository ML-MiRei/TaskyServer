using NotificationService.Core.Models;

namespace NotificationService.Application.Abstractions.Services
{
    public interface INotificationSender
    {
        void SendNotification(string userId, NotificationModel notificationModel);
        void SendNotifications(string[] userIds, NotificationModel[] notificationModel);

    }
}
