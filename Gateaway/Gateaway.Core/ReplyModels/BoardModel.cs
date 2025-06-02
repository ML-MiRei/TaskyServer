using Getaway.Core.Enums;

namespace Gateaway.Core.ReplyModels
{
    public class BoardModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public BoardType Type { get; set; }
        public SprintModel[] Sprints { get; set; }
        public StageModel[] Stages { get; set; }
        public BoardTaskModel[] Tasks { get; set; }

    }
}
