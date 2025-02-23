using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class RoleModel
    {
        private RoleModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }


        public static Result<RoleModel> Create(int id, string name)
        {
            var res = new ResultFactory<RoleModel>(); 
            res.SetResult(new RoleModel(id, name));
            return res.Create();
        }
    }
}
