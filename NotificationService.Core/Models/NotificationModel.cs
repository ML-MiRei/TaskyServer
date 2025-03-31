namespace NotificationService.Core.Models
{
    public class NotificationModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public NotificationObjectValOb Object { get; set; }

        public NotificationModel(string title, string text, DateTime createdDate, NotificationObjectValOb obj, int? id = null)
        {
            Id = id;
            Title = title;
            Text = text;
            CreatedDate = createdDate;
            Object = obj;
        }
    }
}
