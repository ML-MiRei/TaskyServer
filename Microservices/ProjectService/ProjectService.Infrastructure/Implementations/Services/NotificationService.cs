using ProjectService.Application.Abstractions.Services;
using ProjectService.Application.Dtos;
using ProjectService.Infrastructure.Kafka;

namespace ProjectService.Infrastructure.Implementations.Services
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
