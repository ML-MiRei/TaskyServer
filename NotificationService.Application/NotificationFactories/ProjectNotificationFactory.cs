using NotificationService.Core.Enums;
using NotificationService.Core.Models;

namespace NotificationService.Application.NotificationFactories
{
    public class ProjectNotificationFactory : NotificationFactory
    {
        private static  ObjectTypeValOb _type = new ObjectTypeValOb(ObjectTypes.Project);
        protected override ObjectTypeValOb Type => _type;

    }
}
