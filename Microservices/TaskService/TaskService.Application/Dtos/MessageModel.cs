namespace TaskService.Application.Dtos
{
    public class MessageModel
    {
        public string[] UserIds { get; set; }
        public string? ObjectId {  get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public MessageModel(string[] userIds, string message, string title, string? taskId = null)
        {
            UserIds = userIds;
            ObjectId = taskId;
            Message = message;
            Title = title;
        }
    }
}
