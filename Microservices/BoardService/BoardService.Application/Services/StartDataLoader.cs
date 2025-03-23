using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Enums;
using BoardService.Core.Models;

namespace BoardService.Application.Services
{
    public class StartDataLoader(IStagesRepository stagesRepository)
    {
        public async Task AddBaseStages(string boardId, BoardType type)
        {
            if (type == BoardType.Kanban)
            {
                StageModel[] stages = { StageModel.Create(boardId, 0, "сделать", maxTasks: 5).Value,
                                        StageModel.Create(boardId, 1, "в работе", maxTasks: 5).Value,
                                        StageModel.Create(boardId, 2, "готово").Value };
                await stagesRepository.CreateAsync(stages);
            }
            else if (type == BoardType.SCRUM)
            {
                StageModel[] stages = { StageModel.Create(boardId, 0, "сделать").Value,
                                        StageModel.Create(boardId, 1, "в работе").Value,
                                        StageModel.Create(boardId, 2, "тестируется").Value,
                                        StageModel.Create(boardId, 3, "готово").Value };
                await stagesRepository.CreateAsync(stages);
            }
        }
    }
}
