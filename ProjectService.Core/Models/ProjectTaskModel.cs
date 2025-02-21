using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class ProjectTaskModel:BaseModel
    {
        public const int MAX_LENGHT_TITLE = 100;
        public const int MAX_LENGHT_DESCRIPTION = 500;



        public int? Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public Guid? Executor { get; private set; }
        //public Guid Project { get; private set; }
        public int? Sprint { get; private set; }
        public int? StatusId { get; private set; }
        //public List<FileModel> Files { get; private set; }

        private ProjectTaskModel(int? id,
                                 string title,
                                 string? description,
                                 //Guid? executor,
                                 //Guid project,
                                 //int? sprint,
                                 int? status = 0)
                                 //List<FileModel> files)
        {
            Id = id;
            Title = title;
            Description = description;
            //Executor = executor;
            //Project = project;
            //Sprint = sprint;
            StatusId = status;
            //Files = files;
        }

        public static Result<ProjectTaskModel> Create(
                                 //Guid project,
                                 string title,
                                 int? status,
                                 int? id = null,
                                 string? description = "")
                                 //Guid? executor = null,
                                 //int? sprint = null,
                                 //List<FileModel> files = null)
        {
            var res = new ResultFactory<ProjectTaskModel>();

            if (string.IsNullOrEmpty(title))
                res.AddError("Заголовок не может быть пустым");
            else
            {
                //res.SetResult(new ProjectTaskModel(id, title, description, executor, project, sprint, status, files));
                res.SetResult(new ProjectTaskModel(id, title, description, status));

            }

            return res.Create();
        }


        //public bool AddFile(string filePath)
        //{
        //    var res = FileModel.Create(null, filePath);
        //    if (res.IsSuccess)
        //        Files.Add(res.Value);

        //    return res.IsSuccess;
        //}

        public void SetStatus(int status) => StatusId = status;
        //public void SetSprint(int? id) => Sprint = id;
        //public void SetExecutor(Guid? id) => Executor = id;
        public Result SetDescription(string descr)
        {
            descr = descr.Trim();
            if(descr.Length > MAX_LENGHT_DESCRIPTION)
                return new Result([$"Длина описания не должна превышать {MAX_LENGHT_DESCRIPTION}"]);

            Description = descr;
            return new Result();
        }
        public Result SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title.Trim()))
                return new Result([$"Заголовок не должен быть пустым"]);

            if (title.Length > MAX_LENGHT_TITLE)
                return new Result([$"Длина заголовка не должна превышать {MAX_LENGHT_TITLE}"]);

            Title = title.Trim();
            return new Result();
        }


    }
}
