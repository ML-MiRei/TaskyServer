using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Models;
using BoardService.Infrastructure.Database;
using BoardService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardService.Infrastructure.Implementations.Repositories
{
    public class StagesRepository(BoardDbContext context) : IStagesRepository
    {

        public async Task<int> CreateAsync(StageModel stageModel)
        {
            var stage = new ExecutionStageEntity
            {
                BoardId = stageModel.BoardId,
                Name = stageModel.Name,
                MaxTasksCount = stageModel.MaxTasks,
                Queue = stageModel.Queue
            };

            await context.Stages.AddAsync(stage);
            await context.SaveChangesAsync();

            return stage.Id;
        }

        public async Task CreateAsync(StageModel[] stageModels)
        {
            List<ExecutionStageEntity> executionStages = new List<ExecutionStageEntity>();

            foreach (StageModel stageModel in stageModels)
            {
                executionStages.Add(new ExecutionStageEntity
                {
                    BoardId = stageModel.BoardId,
                    Name = stageModel.Name,
                    MaxTasksCount = stageModel.MaxTasks,
                    Queue = stageModel.Queue
                });

            }

            await context.Stages.AddRangeAsync(executionStages);
            await context.SaveChangesAsync();
        }


        public async Task<int> UpdateAsync(StageModel stageModel)
        {
            var stage = await context.Stages.FindAsync(stageModel.Id);

            Task task = null;

            if (stageModel.Queue != stage.Queue)
            {
                task = Task.Run(new Action(() =>
                {
                    var updatingStages = context.Stages.Where(s => s.Queue >= stageModel.Queue);

                    foreach (var updatingStage in updatingStages)
                    {
                        updatingStage.Queue++;
                    }

                    context.UpdateRange(updatingStages);
                }));

                stage.Queue = stageModel.Queue;
            }

            stage.Name = stageModel.Name;
            stage.MaxTasksCount = stageModel.MaxTasks;

            Task.WaitAll(task);

            await context.SaveChangesAsync();

            return stage.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var stage = await context.Stages.FindAsync(id);


            Task task = Task.Run(new Action(() =>
            {
                var updatingStages = context.Stages.Where(s => s.Queue > stage.Queue);

                foreach (var updatingStage in updatingStages)
                {
                    updatingStage.Queue--;
                }

                context.UpdateRange(updatingStages);
            }));

            context.Stages.Remove(stage);

            Task.WaitAll(task);

            await context.SaveChangesAsync();

            return stage.Id;
        }

        public async Task<StageModel> GetAsync(int id)
        {
            var stage = await context.Stages.AsNoTracking().FirstAsync(s => s.Id == id);

            return StageModel.Create(stage.BoardId, stage.Queue, stage.Name, stage.Id, stage.MaxTasksCount).Value;
        }

        public async Task<int?> GetPrevStageIdAsync(int id)
        {
            var stage = await context.Stages.AsNoTracking().FirstAsync(s => s.Id == id);
            var prevStage = await context.Stages.AsNoTracking().Where(s => s.BoardId == stage.BoardId && (stage.Queue - s.Queue) == 1).FirstOrDefaultAsync();

            return prevStage?.Id;
        }


        public async Task<List<StageModel>> GetAllAsync(string boardId)
        {
            var stages = context.Stages
                .AsNoTracking()
                .Where(s => s.BoardId == boardId)
                .Select(s => StageModel.Create(s.BoardId, s.Queue, s.Name, s.Id, s.MaxTasksCount).Value)
                .ToList();

            return stages;
        }
    }
}
