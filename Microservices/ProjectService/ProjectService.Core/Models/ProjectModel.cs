using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class ProjectModel: BaseModel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        //public List<ProjectTaskModel?> Tasks { get; }
        //public List<SprintModel> Sprints { get; }

        //public List<UserModel> Members { get; }

        private ProjectModel(Guid? id,
                            string name,
                            string description)
                            //List<ProjectTaskModel> tasks,
                            //List<SprintModel> sprints,
                            //List<UserModel> members)
        {
            //Tasks = tasks;
            //Sprints = sprints;
            //Members = members;
            Id = id.HasValue? Guid.NewGuid() : id.Value;
            Name = name;
        }


        public static Result<ProjectModel> Create(string name,
                            //List<UserModel> members = null,
                            Guid? id = null,
                            string description = null)
                            //List<ProjectTaskModel?> tasks = null,
                            //List<SprintModel?> sprints = null)
        {
            var res = new ResultFactory<ProjectModel>();

            if (string.IsNullOrEmpty(name))
                res.AddError("Название не может быть пустым");

            id = id == null ? Guid.NewGuid() : id;

            //res.SetResult(new ProjectModel(id, name, picturePath, tasks, sprints, members));
            res.SetResult(new ProjectModel(id, name, description));
            return res.Create();
        }

        //public bool AddMember(UserModel userVO)
        //{
        //    if(userVO == null)
        //        return false;

        //    Members.Add(userVO);
        //    return true;
        //}

        //public bool AddSprint(SprintModel sprint)
        //{
        //    if (sprint  == null)
        //        return false;

        //    Sprints.Add(sprint);
        //    return true;
        //}

        //public bool AddTask(ProjectTaskModel task)
        //{
        //    if (task == null)
        //        return false;

        //    Tasks.Add(task);
        //    return true;
        //}

        //public bool SetName(string name)
        //{
        //    name = name.Trim();
        //    if (string.IsNullOrEmpty(name))
        //        return false;

        //    Name = name;
        //    return true;
        //}

        //public bool SetId(Guid? id)
        //{
        //    if (id == null)
        //        return false ;

        //    Id = id;
        //    return true;
        //}

    }
}
