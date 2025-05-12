using Getaway.Core.Enums;

namespace Gateaway.Core.ReplyModels
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public int Gender { get; set; }
        public ProjectMemberRoles? ProjectRole { get; set; }
    }
}
