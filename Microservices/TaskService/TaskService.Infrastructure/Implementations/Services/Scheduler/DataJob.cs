using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TaskService.Application.Abstractions.Services;

namespace TaskService.Infrastructure.Implementations.Services.Scheduler
{
    public class DataJob(IServiceScopeFactory serviceScopeFactory) : ScheduleEvents, IJob
    {
        public override event DailyEventHandler DailyEvent;

        public Task Execute(IJobExecutionContext context)
        {
            DailyEvent?.Invoke(DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}
