using TaskService.Core.Enums;

namespace TaskService.Core.Models
{
    public class ExecutionModel
    {
        public int? Id { get; }
        public string? TaskId { get; }
        public string UserId { get; }
        public DateTime DateStart { get; }
        public ExecutionStatus Status { get; set; }

        public ExecutionModel(string? taskId, string userId, int statusId, DateTime? dateStart = null, int? id = null)
        {
            Id = id;
            TaskId = taskId;
            UserId = userId;
            Status = (ExecutionStatus)statusId;
            DateStart = dateStart.HasValue ? dateStart.Value : DateTime.UtcNow;
        }

    }
}
