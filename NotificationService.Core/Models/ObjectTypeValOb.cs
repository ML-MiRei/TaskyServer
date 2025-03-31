using NotificationService.Core.Enums;

namespace NotificationService.Core.Models
{
    public class ObjectTypeValOb
    {
        public int Id { get; }
        public string TypeName { get; }

        public ObjectTypeValOb(ObjectTypes type)
        {
            Id = (int)type;
            TypeName = type.ToString();
        }
    }
}
