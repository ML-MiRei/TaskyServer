namespace Getaway.Presentation.Hubs
{
    public class UserConnections
    {
        public List<UserConnection> ListUserConnections = new List<UserConnection>();

        public bool IsConnectedUser(Guid userId)
        {
            return ListUserConnections.Any(c => c.Id == userId);
        }

        public string GetUserConnectionId(Guid userId)
        {
            return ListUserConnections.FirstOrDefault(c => c.Id == userId).ConnectionId;
        }

        public void RemoveUserConnectionId(string connectionId)
        {
            if (IsConnectedUser(Guid.Parse(connectionId)))
            {
                ListUserConnections.Remove(ListUserConnections.First(u => u.ConnectionId == connectionId));
            }
        }

    }
    public class UserConnection
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
    }

}
