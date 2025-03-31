using Microsoft.EntityFrameworkCore;
using TaskService.Application.Abstractions.Services;
using TaskService.Application.Dtos;
using TaskService.Infrastructure.Database;

namespace TaskService.Infrastructure.Implementations.Services
{
    public class AutoNotificationsService(TasksDbContext context, INotificationService notificationService) : IAutoNotificationsService
    {
        private const string TITLE = "Ежедневное уведомление";
        private const string COUNT_TASK_MESSAGE_TEMPLATE = "Количество задач в работе: {count}. Не забудьте выполнить их все";
        private const string COMPLETION_ONE_REMIND_MESSAGE_TEMPLATE = "Срок выполнения задачи {title} ближется к концу. Поспешите её вполнить";
        private const string COMPLETION_MANY_REMIND_MESSAGE_TEMPLATE = "Срок выполнения задач {title} ближется к концу. Поспешите их вполнить";

        private const int COUNT_DAYS_REMIND = 1;

        public void SendCountTaskInWorkReminder(DateTime date)
        {
            var notifications = context.Executions.AsNoTracking()
                .GroupBy(u => new { u.UserId, u.TaskId })
                .Select(e => new { e.Key.UserId, e.Key.TaskId, count = e.Count() })
                .Where(e => e.count % 2 == 1)  //когда количество нечётное, статус = in process 
                .GroupBy(e => e.UserId)
                .Select(e => new { e.Key, message = COUNT_TASK_MESSAGE_TEMPLATE.Replace("{count}", e.Count().ToString()) });

            foreach (var notification in notifications)
            {
                notificationService.SendNotification(new MessageModel([notification.Key], notification.message, TITLE));
            }
        }

        public void SendTaskСompletionReminder(DateTime date)
        {
            var notifications = context.Executions.AsNoTracking()
               .Include(e => e.Task)
               .Where(e => e.Task.DateEnd != null && DateTime.UtcNow.AddDays(COUNT_DAYS_REMIND) == e.Task.DateEnd)
               .GroupBy(u => new { u.UserId, u.Task })
               .Select(e => new { e.Key.UserId, e.Key.Task, count = e.Count() })
               .Where(e => e.count % 2 == 1)  //когда количество нечётное, статус = in process 
               .GroupBy(e => e.UserId)
               .Select(e => new
               {
                   e.Key,
                   message = (e.Count() == 1 ? COMPLETION_ONE_REMIND_MESSAGE_TEMPLATE : COMPLETION_MANY_REMIND_MESSAGE_TEMPLATE)
                                                .Replace("{title}", string.Join(", ", e.Select(t => $"{t.Task.Title}")))
               });

            foreach (var notification in notifications)
            {
                notificationService.SendNotification(new MessageModel([notification.Key], notification.message, TITLE));
            }
        }
    }
}
