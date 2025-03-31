namespace NotificationService.Infrastructure.Database
{
    public class DbConnectionOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string GetConnectionString()
        {
            return $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password}";
        }
    }
}
