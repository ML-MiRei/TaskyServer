namespace NotificationService.Core.Models
{
    public class NotificationObjectValOb
    {
        public string Id { get; }

        public ObjectTypeValOb ObjectType { get; }

        public NotificationObjectValOb(string id, ObjectTypeValOb objectType)
        {
            Id = id;
            ObjectType = objectType;
        }
    }
}
