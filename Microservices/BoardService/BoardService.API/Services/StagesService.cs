using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Common;
using BoardService.Core.Enums;
using BoardService.Core.Models;
using Grpc.Core;

namespace BoardService.API.Services
{
    public class StagesService(ILogger<StagesService> logger, IBoardsRepository boardsRepository, IStagesRepository stagesRepository, ITasksRepository tasksRepository) : Stages.StagesBase
    {

        public async override Task<CreateStageReply> CreateStage(CreateStageRequest request, ServerCallContext context)
        {
            var res = StageModel.Create(request.BoardId, request.Queue, request.Name, maxTasks: request.MaxTasksCount);

            if (res.IsError)
            {
                logger.LogDebug(res.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, res.StringErrors));
            }

            var stageModel = res.Value;

            try
            {
                var board = await boardsRepository.GetAsync(stageModel.BoardId);
                if (board.Type != Core.Enums.BoardType.Kanban && (stageModel.MaxTasks.HasValue || stageModel.MaxTasks == 0))
                {
                    logger.LogDebug(ErrorMessagesConsts.INVALID_BOARD_TYPE_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.InvalidArgument, ErrorMessagesConsts.INVALID_BOARD_TYPE_ERROR_MESSAGE));
                }

                var reply = await stagesRepository.CreateAsync(stageModel);
                return new CreateStageReply { StageId = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }


       


        public async override Task<DeleteStageReply> DeleteStage(DeleteStageRequest request, ServerCallContext context)
        {
            try
            {
                var projectTasks = tasksRepository.GetAllByStageAsync(request.StageId);
                var prevStageId = stagesRepository.GetPrevStageIdAsync(request.StageId);

                Task.WaitAll(projectTasks, prevStageId);

                List<Task> tasks = new List<Task>();

                foreach (TaskModel projectTask in projectTasks.Result)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        projectTask.StageId = prevStageId.Result;
                        tasksRepository.ChangeStageAsync(projectTask);
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                var reply = await stagesRepository.DeleteAsync(request.StageId);
                return new DeleteStageReply { StageId = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public async override Task<GetStageReply> GetStage(GetStageRequest request, ServerCallContext context)
        {
            try
            {
                var reply = await stagesRepository.GetAsync(request.StageId);
                if (reply == null)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                return new GetStageReply
                {
                    StageId = reply.Id.Value,
                    MaxTasksCount = reply.MaxTasks,
                    Name = reply.Name,
                    Queue = reply.Queue
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetStagesReply> GetStagesByBoardId(GetStagesRequest request, ServerCallContext context)
        {
            try
            {
                var response = await stagesRepository.GetAllAsync(request.BoardId);
                if (response.Count == 0)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                var reply = new GetStagesReply();
                reply.Stages.AddRange(response.Select(s => new StageInfo { StageId = s.Id.Value, Name = s.Name, Queue = s.Queue, MaxTasksCount = s.MaxTasks }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<UpdateStageReply> UpdateStage(UpdateStageRequest request, ServerCallContext context)
        {
            var res = StageModel.Create(request.BoardId, request.Queue, request.Name, request.StageId, maxTasks: request.MaxTasksCount);

            if (res.IsError)
            {
                logger.LogDebug(res.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, res.StringErrors));
            }

            var stageModel = res.Value;

            try
            {
                var board = await boardsRepository.GetAsync(stageModel.BoardId);
                if (board.Type != Core.Enums.BoardType.Kanban && (stageModel.MaxTasks.HasValue || stageModel.MaxTasks == 0))
                {
                    logger.LogDebug(ErrorMessagesConsts.INVALID_BOARD_TYPE_ERROR_MESSAGE);
                    stageModel.ClearMaxTasks();
                }

                var reply = await stagesRepository.UpdateAsync(stageModel);
                return new UpdateStageReply { StageId = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }
    }
}
