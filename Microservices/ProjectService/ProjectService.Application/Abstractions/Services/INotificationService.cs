namespace ProjectService.Application.Abstractions.Services
{
    public interface INotificationService
    {
        public void SendMessageAsync(string title, string body);
    }
}
