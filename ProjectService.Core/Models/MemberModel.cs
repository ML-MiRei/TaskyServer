using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class MemberModel: BaseModel
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public string? PicturePath { get; private set; }
        public int RoleID {  get; private set; } 


        public MemberModel(Guid? id, int roleId, string? name = null, string? picturePath = null)
        {
            Id = id.HasValue? id.Value : Guid.NewGuid();
            Name = name;
            PicturePath = picturePath;
            RoleID = roleId;
        }


        public void SetName(string name) => Name = name;
        public void SetPicturePath(string picturePath) => PicturePath = picturePath;
    }
}
