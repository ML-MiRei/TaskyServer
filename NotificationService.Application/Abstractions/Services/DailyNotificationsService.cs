namespace NotificationService.Application.Abstractions.Services
{
    public abstract class DailyNotificationsService
    {
        public delegate void DailyNotificationsHandler(DateTime date);
        public abstract event DailyNotificationsHandler DailyNotificationsEvent;
    }
}
