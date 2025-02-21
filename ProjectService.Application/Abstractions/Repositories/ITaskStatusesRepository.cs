using ProjectService.Application.Common.Base;
using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Repositories
{
    public interface ITaskStatusesRepository: IBaseRepository<StatusModel, int>
    {
    }
}
