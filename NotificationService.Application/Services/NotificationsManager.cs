using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Application.Abstractions.Services;

namespace NotificationService.Application.Services
{
    public class NotificationsManager
    {
        private DailyNotificationsService _dailyNotificationsService;

        public NotificationsManager(DailyNotificationsService dailyNotificationsService, INotificationsRepository notificationsRepository)
        {
            _dailyNotificationsService = dailyNotificationsService;
            _dailyNotificationsService.DailyNotificationsEvent += SendSprintsNotifications;
            _dailyNotificationsService.DailyNotificationsEvent += SendTasksNotifications;
        }

        private void SendTasksNotifications(DateTime date)
        {


        }

        private void SendSprintsNotifications(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
