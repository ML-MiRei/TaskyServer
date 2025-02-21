using Microsoft.Extensions.Logging;
using ProjectService.Core.Common;

namespace ProjectService.Application.Common.Base
{
    public abstract class GetCrudCommandsBase<TModel, TId, TRepository>(TRepository repository, ILogger logger) where TModel : BaseModel where TRepository : IBaseRepository<TModel, TId>
    {


        public async Task<Result<List<TModel>>> GetAllTasksAsync(Guid parentId)
        {
            var res = new ResultFactory<List<TModel>>();

            try
            {
                var models = await repository.GetAllAsync(parentId);
                res.SetResult(models);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка получения данных. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<TModel>> GetTaskAsync(TId id)
        {
            var res = new ResultFactory<TModel>();

            try
            {
                var model = await repository.GetByIdAsync(id);
                res.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка получения данных. Повторите попытку позже");
            }

            return res.Create();
        }

    }
}
