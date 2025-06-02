namespace Gateaway.Core.RequestModels.Boards
{
    public class AddTasksRequest
    {
        public int? StageId { get; set; }
        public string[] TasksIds { get; set; }
    }
}
