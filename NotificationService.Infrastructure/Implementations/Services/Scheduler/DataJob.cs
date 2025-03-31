using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Abstractions.Services;
using Quartz;

namespace NotificationService.Infrastructure.Implementations.Services.Scheduler
{
    public class DataJob(IServiceScopeFactory serviceScopeFactory) : DailyNotificationsService, IJob
    {
        public override event DailyNotificationsHandler DailyNotificationsEvent;

        public Task Execute(IJobExecutionContext context)
        {
            DailyNotificationsEvent?.Invoke(DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}
