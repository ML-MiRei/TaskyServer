namespace ProjectService.Application.Dtos
{
    public class MessageModel
    {
        public string[] UserIds { get; set; }
        public string? TaskId {  get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public MessageModel(string[] userIds, string message, string title, string? taskId = null)
        {
            UserIds = userIds;
            TaskId = taskId;
            Message = message;
            Title = title;
        }
    }
}
