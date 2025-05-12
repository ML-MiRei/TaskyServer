using Getaway.Core.Enums;

namespace Gateaway.Core.ReplyModels
{
    public class BoardModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public BoardType Type { get; set; }
        public List<SprintModel> Sprints { get; set; }
        public List<StageModel> Stages { get; set; }
        public List<TaskModel> Tasks { get; set; }

    }
}
