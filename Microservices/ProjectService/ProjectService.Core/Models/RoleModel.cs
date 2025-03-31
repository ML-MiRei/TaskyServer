using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class RoleModel
    {
        public RoleModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

    }
}
