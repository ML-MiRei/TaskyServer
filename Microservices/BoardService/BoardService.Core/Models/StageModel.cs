using BoardService.Core.Common;

namespace BoardService.Core.Models
{
    public class StageModel
    {
        public int? Id { get; private set; }
        public string Name { get; private set; }
        public int? MaxTasks{ get; private set; }
        public int Queue { get; private set; }
        public string BoardId { get; private set; }


        private StageModel(int? id, string name, int queue, int? maxTasks, string boardId)
        {
            Id = id;
            Name = name;
            MaxTasks = maxTasks;
            BoardId = boardId;
            Queue = queue;
        }

        public void ClearMaxTasks()
        {
            MaxTasks = null;
        }


        public static Result<StageModel> Create(string boardId, int queue, string name, int? id = null, int? maxTasks = null)
        {
            var res = new ResultFactory<StageModel>();

            if (string.IsNullOrEmpty(name))
                res.AddError("Название не может быть пустым");

            res.SetResult(new StageModel(id, name, queue, maxTasks, boardId));
            return res.Create();
        }
    }
}
