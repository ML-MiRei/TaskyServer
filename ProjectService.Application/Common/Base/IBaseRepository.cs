using ProjectService.Core.Common;

namespace ProjectService.Application.Common.Base
{
    public interface IBaseRepository<TModel, TId> where TModel : BaseModel
    {
        public Task<TId> CreateAsync( TModel model);
        public Task<TId> DeleteAsync(TId id);
        public Task<TId> UpdateAsync( TModel model);
        public Task<TModel> GetByIdAsync(TId id);
        public Task<List<TModel>> GetAllAsync(Guid parentId);

    }
}
