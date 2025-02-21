using Microsoft.Extensions.Logging;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Application.Common.Base;
using ProjectService.Application.DTOs;
using ProjectService.Core.Common;
using ProjectService.Core.Models;

namespace ProjectService.Application.Services
{
    public class SprintsService(ILogger<SprintsService> logger, ISprintsRepository sprintTasksRepository
        ): GetCrudCommandsBase<SprintModel, (Guid, int), ISprintsRepository>(sprintTasksRepository, logger)
    {

        //public async Task<Result<List<SprintModel>>> GetAllTasksAsync(Guid projectId)
        //{
        //    var res = new ResultFactory<List<SprintModel>>();

        //    try
        //    {
        //        var sprints = await sprintTasksRepository.GetAllAsync(projectId);
        //        res.SetResult(sprints);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex.Message);
        //        res.AddError("Ошибка получения данных. Повторите попытку позже");
        //    }

        //    return res.Create();
        //}

        //public async Task<Result<SprintModel>> GetTaskAsync(Guid projectId, int sprintId)
        //{
        //    var res = new ResultFactory<SprintModel>();

        //    try
        //    {
        //        var task = await sprintTasksRepository.GetByIdAsync(projectId, sprintId);
        //        res.SetResult(task);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex.Message);
        //        res.AddError("Ошибка получения данных. Повторите попытку позже");
        //    }

        //    return res.Create();
        //}




        public async Task<Result<SprintModel>> CreateAsync(SprintDTO sprint
            )
        {
            var res = new ResultFactory<SprintModel>();

            var sprintRes = SprintModel.Create(sprint.ProjectId, sprint.StartDate, sprint.EndDate);

            if (!sprintRes.IsSuccess)
            {
                res.AddError(sprintRes.Errors.ToArray());
                return res.Create();
            }

            try
            {
                var newSprint = sprintRes.Value;
                var projectId = await sprintTasksRepository.CreateAsync(ref newSprint);
                res.SetResult(newSprint);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка сохранения. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<SprintModel>> UpdateAsync(SprintDTO sprint)
        {
            var res = new ResultFactory<SprintModel>();

            var sprintRes = SprintModel.Create(sprint.ProjectId, sprint.StartDate, sprint.EndDate);


            if (!sprintRes.IsSuccess)
            {
                res.AddError(sprintRes.Errors.ToArray());
                return res.Create();
            }

            try
            {
                var newTask = sprintRes.Value;
                var projectId = await sprintTasksRepository.UpdateAsync(ref newTask);
                res.SetResult(newTask);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка сохранения. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<(Guid, int)>> DeleteAsync(Guid projectId, int taskId)
        {
            var res = new ResultFactory<(Guid, int)>();

            try
            {
                var deletedProjectIId = await sprintTasksRepository.DeleteAsync((projectId, taskId));
                res.SetResult(deletedProjectIId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка удаления. Повторите попытку позже");
            }

            return res.Create();
        }
    }
}
