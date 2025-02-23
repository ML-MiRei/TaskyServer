using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class FileModel: BaseModel
    {
        public int? Id { get; }
        public string Path { get; }

        private FileModel(int? id, string path)
        {
            Id = id;
            Path = path;
        }

        public static Result<FileModel> Create(int? id, string path)
        {
            var res = new ResultFactory<FileModel>();

            if (string.IsNullOrEmpty(path))
            {
                res.AddError("Неверный путь файла");
                return res.Create();
            }

            res.SetResult(new FileModel(id, path));
            return res.Create();
        }

    }
}
