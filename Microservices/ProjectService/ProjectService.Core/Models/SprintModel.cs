using ProjectService.Core.Common;

namespace ProjectService.Core.Models
{
    public class SprintModel : BaseModel
    {
        public int? Id { get; }
        public DateTime DateStart { get; }
        public DateTime DateEnd { get; }
        public Guid? ProjectId { get; }
        private SprintModel(int? id, Guid? project, DateTime startDate, DateTime endDate)
        {
            Id = id;
            DateStart = startDate;
            ProjectId = project;
            DateEnd = endDate;
        }

        public static Result<SprintModel> Create(DateTime startDate, DateTime endDate, int? id = null, Guid? project = null )
        {
            var res = new ResultFactory<SprintModel>();

            if (endDate < startDate)
                res.AddError("Дата окончания не может быть меньше даты начала");

            if (startDate == default)
                res.AddError("Неверное значение даты начала спринта");

            if (endDate == default)
                res.AddError("Неверное значение даты окончания спринта");

            res.SetResult(new SprintModel(id, project, startDate, endDate));
            return res.Create();
        }

    }
}
