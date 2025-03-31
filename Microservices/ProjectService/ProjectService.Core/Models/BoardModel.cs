using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class BoardModel
    {
        public string BoardId { get; }
        public string ProjectId { get; }

        public BoardModel(string boardId, string projectId)
        {
            BoardId = boardId;
            ProjectId = projectId;
        }
    }
}
