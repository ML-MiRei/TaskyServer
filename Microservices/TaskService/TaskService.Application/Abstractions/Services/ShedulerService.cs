namespace TaskService.Application.Abstractions.Services
{
    public class ShedulerService
    {
        private static ScheduleEvents _scheduleEvents;
        private static IAutoNotificationsService _autoNotificationsService;

        public ShedulerService(ScheduleEvents scheduleEvents, IAutoNotificationsService autoNotificationsService)
        {
            _autoNotificationsService = autoNotificationsService;
            _scheduleEvents = scheduleEvents;
        }

        public void SetAutoNotifications()
        {
            _scheduleEvents.DailyEvent += _autoNotificationsService.SendCountTaskInWorkReminder;
            _scheduleEvents.DailyEvent += _autoNotificationsService.SendTaskСompletionReminder;
        }
    }
}
