using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Models;
using BoardService.Infrastructure.Database;
using BoardService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Implementations.Repositories
{
    public class TasksRepository(BoardDbContext context) : ITasksRepository
    {
        public async Task<string> AddAsync(TaskModel[] taskModels)
        {
            var tasks = taskModels.Select(t => new TaskEntity
            {
                BoardId = t.BoardId,
                Id = t.TaskId,
                SprintId = t.SprintId,
                StageId = t.StageId
            });

            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();

            return taskModels[0].BoardId;
        }

        public async Task<string> ChangeStageAsync(TaskModel taskModel)
        {
            var sprint = await context.Tasks.FindAsync(taskModel.TaskId);

            sprint.StageId = taskModel.StageId;

            //context.Tasks.Update(sprint);
            await context.SaveChangesAsync();

            return sprint.Id;
        }


        public async Task<int> ChangeSprintAsync(TaskModel[] taskModel)
        {
            var tasksIds = taskModel.Select(t => t.TaskId);
            var sprintId = taskModel[0].SprintId;

            var tasks = context.Tasks.Where(t => tasksIds.Contains(t.Id));

            var stage = await context.Stages.AsNoTracking()
                                            .Where(s => s.BoardId == taskModel[0].BoardId)
                                            .OrderBy(s => s.Queue)
                                            .FirstAsync();

            foreach (var task in tasks)
            {
                task.SprintId = sprintId;
                task.StageId = stage.Id;
            }

            //context.Tasks.UpdateRange(sprints);
            await context.SaveChangesAsync();

            return sprintId.Value;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var task = await context.Tasks.FindAsync(id);

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return id;
        }


        public async Task<List<TaskModel>> GetAllByBoardAsync(string boardId)
        {
            var tasks = context.Tasks
                .AsNoTracking()
                .Where(s => s.BoardId == boardId)
                .Select(s => new TaskModel(s.Id, s.BoardId, s.StageId, s.SprintId))
                .ToList();

            return tasks;
        }

        public async Task<List<TaskModel>> GetAllBySprintAsync(int sprintId)
        {
            var tasks = context.Tasks
                .AsNoTracking()
                .Where(s => s.SprintId == sprintId)
                .Select(s => new TaskModel(s.Id, s.BoardId, s.StageId, s.SprintId))
                .ToList();

            return tasks;
        }

        public async Task<List<TaskModel>> GetAllByStageAsync(int stageId)
        {
            var tasks = context.Tasks
                .AsNoTracking()
                .Where(s => s.StageId == stageId)
                .Select(s => new TaskModel(s.Id, s.BoardId, s.StageId, s.SprintId))
                .ToList();

            return tasks;
        }
    }
}
