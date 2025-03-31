
namespace TaskService.Application.Abstractions.Services
{
    public interface IAutoNotificationsService
    {
        public abstract void SendTaskСompletionReminder(DateTime date);
        public abstract void SendCountTaskInWorkReminder(DateTime date);

    }
}
