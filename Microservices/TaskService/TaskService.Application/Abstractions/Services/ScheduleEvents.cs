namespace TaskService.Application.Abstractions.Services
{
    public abstract class ScheduleEvents
    {
        public delegate void DailyEventHandler(DateTime date);
        public abstract event DailyEventHandler DailyEvent;
    }
}
