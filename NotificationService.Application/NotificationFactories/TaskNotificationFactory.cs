using NotificationService.Core.Enums;
using NotificationService.Core.Models;

namespace NotificationService.Application.NotificationFactories
{
    public class TaskNotificationFactory : NotificationFactory
    {
        private static  ObjectTypeValOb _type = new ObjectTypeValOb(ObjectTypes.Task);
        protected override ObjectTypeValOb Type => _type;

    }
}
