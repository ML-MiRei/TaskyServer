using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Common;
using BoardService.Core.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BoardService.API.Services
{
    public class SprintsService(ILogger<SprintsService> logger, IBoardsRepository boardsRepository, ISprintsRepository sprintsRepository) : Sprints.SprintsBase
    {

        public async override Task<CreateSprintReply> CreateSprint(CreateSprintRequest request, ServerCallContext context)
        {
            var sprintModel = SprintModel.Create(request.DateStart.ToDateTime(), request.DateEnd.ToDateTime(), request.BoardId);

            if (sprintModel.IsError)
            {
                logger.LogDebug(sprintModel.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, sprintModel.StringErrors));
            }

            try
            {
                var board = await boardsRepository.GetAsync(sprintModel.Value.BoardId);
                if (board.Type != Core.Enums.BoardType.SCRUM)
                {
                    logger.LogDebug(ErrorMessagesConsts.INVALID_BOARD_TYPE_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.InvalidArgument, ErrorMessagesConsts.INVALID_BOARD_TYPE_ERROR_MESSAGE));
                }

                var reply = await sprintsRepository.CreateAsync(sprintModel.Value);
                return new CreateSprintReply { Id = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<DeleteSprintReply> DeleteSprint(DeleteSprintRequest request, ServerCallContext context)
        {
            try
            {
                var reply = await sprintsRepository.DeleteAsync(request.Id);
                return new DeleteSprintReply { Id = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public async override Task<GetSprintReply> GetSprint(GetSprintRequest request, ServerCallContext context)
        {
            try
            {
                var reply = await sprintsRepository.GetAsync(request.Id);
                if (reply == null)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                return new GetSprintReply
                {
                    Id = reply.Id.Value,
                    DateEnd = Timestamp.FromDateTimeOffset(reply.DateEnd),
                    DateStart = Timestamp.FromDateTimeOffset(reply.DateStart)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }

        }

        public async override Task<GetSprintsReply> GetSprints(GetSprintsRequest request, ServerCallContext context)
        {
            try
            {
                var reply = await sprintsRepository.GetAllAsync(request.BoardId);
                if (reply.Count == 0)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                var res = new GetSprintsReply();
                res.Sprints.AddRange(reply.Select(s => new SprintInfo
                {
                    Id = s.Id.Value,
                    DateStart = Timestamp.FromDateTimeOffset(s.DateStart),
                    DateEnd = Timestamp.FromDateTimeOffset(s.DateEnd)
                }));
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<UpdateSprintReply> UpdateSprint(UpdateSprintRequest request, ServerCallContext context)
        {
            var sprintModel = SprintModel.Create(request.DateStart.ToDateTime(), request.DateEnd.ToDateTime(), id: request.Id);

            if (sprintModel.IsError)
            {
                logger.LogDebug(sprintModel.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, sprintModel.StringErrors));
            }

            try
            {
                var reply = await sprintsRepository.UpdateAsync(sprintModel.Value);
                return new UpdateSprintReply { Id = reply };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }

        }
    }
}
