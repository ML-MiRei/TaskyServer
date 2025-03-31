using NotificationService.Core.Enums;
using NotificationService.Core.Models;

namespace NotificationService.Application.NotificationFactories
{
    public class BoardNotificationFactory : NotificationFactory
    {
        private static  ObjectTypeValOb _type = new ObjectTypeValOb(ObjectTypes.Board);
        protected override ObjectTypeValOb Type => _type;
    }
}
