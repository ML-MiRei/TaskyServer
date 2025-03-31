namespace NotificationService.Core.Models
{
    public class MessageTemplate
    {
        public MessageTemplate(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public string Title { get; set; }
        public string Text { get; set; }

    }
}
