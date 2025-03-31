using NotificationService.Core.Models;

namespace NotificationService.Application.Abstractions.Repositories
{
    public interface INotificationsRepository
    {
        Task<int> CreateAsync(NotificationModel notificationModel);
        Task<int> DeleteAsync(int id);
        Task<List<NotificationModel>> GetAllByUserId(string userId);
    }
}