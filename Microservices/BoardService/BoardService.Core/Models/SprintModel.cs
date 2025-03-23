using BoardService.Core.Common;

namespace BoardService.Core.Models
{
    public class SprintModel
    {
        public int? Id { get; private set; }
        public DateTime DateStart{ get; private set; }
        public DateTime DateEnd{ get; private set; }
        public string BoardId {  get; private set; }
        private SprintModel(int? id, string boardId, DateTime dateStart, DateTime dateEnd)
        {
            Id = id;
            DateStart = dateStart;
            DateEnd = dateEnd;
            BoardId = boardId;
        }

        public static Result<SprintModel> Create( DateTime dateStart, DateTime dateEnd, string boardId = null, int? id = null)
        {
            var res = new ResultFactory<SprintModel>();

            if (dateStart == default || dateEnd == default)
                res.AddError("Дата начала и дата конца не должны отсутствовать");

            if (dateStart > dateEnd)
                res.AddError("Дата начала не должна быть позже даты окончания");

            res.SetResult(new SprintModel(id, boardId, dateStart, dateEnd));
            return res.Create();
        }
    }
}
