using ProjectService.Application.Common.Base;
using ProjectService.Core.Models;

namespace ProjectService.Application.Abstractions.Repositories
{
    public interface IProjectTasksRepository: IBaseRepository<ProjectTaskModel, int>
    {
        public int GetCountAsync(Guid projectId);
        public int GetSuccessAsync(Guid projectId);

    }
}
