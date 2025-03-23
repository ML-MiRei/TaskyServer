namespace BoardService.Core.Models
{
    public class TaskModel
    {
        public string TaskId { get; set; }
        public string BoardId { get; set; }
        public int? StageId { get; set; }
        public int?  SprintId { get; set; }

        public TaskModel(string taskId, string boardId, int? stageId = null, int? sprintId = null)
        {
            TaskId = taskId;
            StageId = stageId;
            SprintId = sprintId;
            BoardId = boardId;
        }
    }
}
