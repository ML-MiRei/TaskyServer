using TaskService.Application.Abstractions.Services;
using TaskService.Application.Dtos;
using TaskService.Infrastructure.Kafka;

namespace TaskService.Infrastructure.Implementations.Services
{
    public class NotificationService(KafkaProducer kafkaProducer) : INotificationService
    {
        public void SendNotification(MessageModel messageModel)
        {
            var key = $"{DateTime.UtcNow.ToString("d")} {messageModel.Title}";
            kafkaProducer.SendMessage(key, messageModel);
        }
    }
}
