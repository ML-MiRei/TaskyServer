using Microsoft.AspNetCore.SignalR;

namespace Gateaway.Application.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            Console.WriteLine(connection.User?.Identity.Name);
            return connection.User?.Identity.Name;
        }
    }
}
