using ProjectService.Application.Dtos;

namespace ProjectService.Application.Abstractions.Services
{
    public interface INotificationService
    {
        void SendNotification(MessageModel messageModel);
    }
}
