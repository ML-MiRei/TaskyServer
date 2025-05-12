using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Core.Models;

namespace NotificationService.API.SignalR.Hubs
{
    [Authorize]
    public class NotificationHub(ILogger<NotificationHub> logger) : Hub, INotificationSender
    {
        public async void SendNotification(string userId, NotificationModel notificationModel)
        {
            try
            {
                await Clients.User(userId).SendAsync("NotificationReceiver", notificationModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public void SendNotification(string[] userIds, NotificationModel[] notificationModel)
        {
            try
            {
                var tasks = new List<Task>();

                for(int i = 0; i < userIds.Length; i++)
                {
                    tasks.Add(Task.Run(() => Clients.User(userIds[i]).SendAsync("NotificationReceiver", notificationModel[i])));
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
