using NotificationService.Core.Enums;
using NotificationService.Core.Models;

namespace NotificationService.Application.NotificationFactories
{
    public abstract class NotificationFactory
    {
        protected abstract ObjectTypeValOb Type {  get; }
        public NotificationModel? Create( MessageTemplate message, string objectId = null, int? id = null)
        {
            var obj = new NotificationObjectValOb(objectId, Type);
            var notification = new NotificationModel(message.Title, message.Text, DateTime.UtcNow, obj, id);

            return notification;
        }
    }
}
