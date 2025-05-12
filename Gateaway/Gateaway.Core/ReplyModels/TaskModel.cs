using Getaway.Core.Enums;

namespace Gateaway.Core.ReplyModels
{
    public class TaskModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<TaskModel> Childs { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEnd { get; set; }
        public int StageId { get; set; }
        public int SprintId { get; set; }
        public UserModel? Executor { get; set; }
        public ExecutionStatus ExecutionStatus { get; set; }
    }
}
