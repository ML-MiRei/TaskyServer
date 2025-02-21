using Microsoft.Extensions.Logging;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Application.Common.Base;
using ProjectService.Application.DTOs;
using ProjectService.Core.Common;
using ProjectService.Core.Models;

namespace ProjectService.Application.Services
{
    public class ProjectTasksService(IProjectTasksRepository projectTasksRepository, ILogger<ProjectTasksService> logger
                                 ) : GetCrudCommandsBase<ProjectTaskModel, (Guid, int), IProjectTasksRepository>(projectTasksRepository, logger)
    {

        //public async Task<Result<List<ProjectTaskModel>>> GetAllTasksAsync(Guid projectId)
        //{
        //    var res = new ResultFactory<List<ProjectTaskModel>>();

        //    try
        //    {
        //        var tasks = await projectTasksRepository.GetAllAsync(projectId);
        //        res.SetResult(tasks);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex.Message);
        //        res.AddError("Ошибка получения данных. Повторите попытку позже");
        //    }

        //    return res.Create();
        //}

        //public async Task<Result<ProjectTaskModel>> GetTaskAsync(Guid projectId, int taskId)
        //{
        //    var res = new ResultFactory<ProjectTaskModel>();

        //    try
        //    {
        //        var task = await projectTasksRepository.GetByIdAsync(projectId, taskId);
        //        res.SetResult(task);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex.Message);
        //        res.AddError("Ошибка получения данных. Повторите попытку позже");
        //    }

        //    return res.Create();
        //}




        public async Task<Result<ProjectTaskModel>> CreateAsync(ProjectTaskDTO projectTask)
        {
            var res = new ResultFactory<ProjectTaskModel>();

            //change logic files
            var taskRes = ProjectTaskModel.Create(
                project: projectTask.Project,
                title: projectTask.Title,
                status: projectTask.Status,
                description: projectTask.Description,
                executor: projectTask.Executor,
                sprint: projectTask.Sprint);

            if (!taskRes.IsSuccess)
            {
                res.AddError(taskRes.Errors.ToArray());
                return res.Create();
            }

            try
            {
                var newTask = taskRes.Value;
                var projectId = await projectTasksRepository.CreateAsync(ref newTask);
                res.SetResult(newTask);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка сохранения. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<ProjectTaskModel>> UpdateAsync(ProjectTaskDTO projectTask)
        {
            var res = new ResultFactory<ProjectTaskModel>();

            var taskRes = ProjectTaskModel.Create(
                  id: projectTask.Id,
                  project: projectTask.Project,
                  title: projectTask.Title,
                  status: projectTask.Status,
                  description: projectTask.Description,
                  executor: projectTask.Executor,
                  sprint: projectTask.Sprint);

            if (!taskRes.IsSuccess)
            {
                res.AddError(taskRes.Errors.ToArray());
                return res.Create();
            }

            try
            {
                var newTask = taskRes.Value;
                var projectId = await projectTasksRepository.UpdateAsync(ref newTask);
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
                var deletedProjectIId = await projectTasksRepository.DeleteAsync((projectId, taskId));
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
