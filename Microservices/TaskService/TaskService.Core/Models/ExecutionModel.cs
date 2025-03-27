using TaskService.Core.Enums;

namespace TaskService.Core.Models
{
    public class ExecutionModel
    {
        public int? Id { get; }
        public string? TaskId { get; }
        public string UserId { get; }
        public DateTime DateStart { get; private set; }
        public ExecutionStatus Status
        {
            get => Status;
            set
            {
                Status = value;
                DateStart = DateTime.UtcNow;
            }
        }

        public ExecutionModel(string? taskId, string userId, ExecutionStatus statusId = ExecutionStatus.Started, DateTime? dateStart = null, int? id = null)
        {
            Id = id;
            TaskId = taskId;
            UserId = userId;
            Status = statusId;
            DateStart = dateStart.HasValue ? dateStart.Value : DateTime.UtcNow;
        }

    }
}
