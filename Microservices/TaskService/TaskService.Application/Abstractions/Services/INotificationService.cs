using TaskService.Application.Dtos;

namespace TaskService.Application.Abstractions.Services
{
    public interface INotificationService
    {
        void SendNotification(MessageModel messageModel);
    }
}
