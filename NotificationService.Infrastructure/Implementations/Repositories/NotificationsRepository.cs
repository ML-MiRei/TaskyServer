using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Core.Models;
using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.Database.Entities;

namespace NotificationService.Infrastructure.Implementations.Repositories
{
    public class NotificationsRepository(NotificationDbContext context) : INotificationsRepository
    {
        public async Task<int> CreateAsync(NotificationModel notificationModel)
        {
            var notification = new NotificationEntity
            {
                CreatedDate = notificationModel.CreatedDate,
                ObjectId = notificationModel.Object.Id,
                ObjectTypeId = notificationModel.Object.TypeId,
                Text = notificationModel.Text,
                Title = notificationModel.Title,
                UserId = notificationModel.UserId
            };

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var notification = await context.Notifications.FindAsync(id);

            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<List<NotificationModel>> GetAllByUserId(string userId)
        {
            var notifications = context.Notifications.AsNoTracking()
                .Where(n => n.UserId == userId)
                .Select(n => new NotificationModel(n.Title, n.Text, n.CreatedDate, n.UserId, new NotificationObjectValOb(n.ObjectId, n.ObjectType.Id, n.ObjectType.Name), n.Id))
                .ToList();

            return notifications;
        }
    }
}
