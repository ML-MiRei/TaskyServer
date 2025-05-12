using Microsoft.AspNetCore.SignalR;

namespace NotificationService.API.SignalR
{
    public class UserProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("userId")?.Value;
        }
    }
}

