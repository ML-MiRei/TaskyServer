using BoardService.Core.Common;
using BoardService.Core.Enums;

namespace BoardService.Core.Models
{
    public class BoardModel
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public BoardType? Type { get; private set; }

        private BoardModel(string id, string title, BoardType? type)
        {
            Id = id;
            Title = title;
            Type = type;
        }

        public static Result<BoardModel> Create(string title, BoardType? type = null, string id = null)
        {
            var res = new ResultFactory<BoardModel>();

            if (string.IsNullOrEmpty(title))
                res.AddError("Название не может быть пустым");

            id = id == null ? Guid.NewGuid().ToString() : id;

            res.SetResult(new BoardModel(id, title, type));
            return res.Create();
        }
    }
}
