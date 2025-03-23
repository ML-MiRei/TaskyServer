using BoardService.Application.Abstractions.Repositories;
using BoardService.Core.Enums;
using BoardService.Core.Models;
using BoardService.Core.Common;
using Grpc.Core;
using BoardService.Application.Services;

namespace BoardService.API.Services
{
    public class BoardsService(IBoardsRepository repository, ILogger<BoardsService> logger, StartDataLoader loader) : Boards.BoardsBase
    {

        public override async Task<CreateBoardReply> CreateBoard(CreateBoardRequest request, ServerCallContext context)
        {
            var boardModel = BoardModel.Create(request.Title, (BoardType)request.Type);

            if (boardModel.IsError)
            {
                logger.LogDebug(boardModel.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, boardModel.StringErrors));
            }

            try
            {
                var reply = await repository.CreateAsync(boardModel.Value);
                await loader.AddBaseStages(reply, (BoardType)request.Type);
                return new CreateBoardReply
                {
                    Id = reply
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<DeleteBoardReply> DeleteBoard(DeleteBoardRequest request, ServerCallContext context)
        {
            Guid res;

            if (!Guid.TryParse(request.Id, out res))
            {
                logger.LogDebug($"{ErrorMessagesConsts.INVALID_ID_ERROR_MESSAGE} id: {request.Id}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, ErrorMessagesConsts.INVALID_ID_ERROR_MESSAGE));
            }

            try
            {
                var reply = await repository.DeleteAsync(request.Id);
                return new DeleteBoardReply
                {
                    Id = reply
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public async override Task<GetBoardReply> GetBoard(GetBoardRequest request, ServerCallContext context)
        {
            Guid res;

            if (!Guid.TryParse(request.Id, out res))
            {
                logger.LogDebug($"{ErrorMessagesConsts.INVALID_ID_ERROR_MESSAGE} id: {request.Id}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, ErrorMessagesConsts.INVALID_ID_ERROR_MESSAGE));
            }

            try
            {
                var reply = await repository.GetAsync(request.Id);

                if (reply == null)
                {
                    logger.LogDebug(ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE);
                    throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
                }

                return new GetBoardReply
                {
                    Id = reply.Id,
                    Title = reply.Title,
                    Type = (int)reply.Type
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetBoardsReply> GetBoards(GetBoardsRequest request, ServerCallContext context)
        {
            var ids = request.Id.ToArray();
            if (ids.Length == 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, ErrorMessagesConsts.INVALID_ID_ERROR_MESSAGE));

            try
            {
                var response = await repository.GetAllAsync(request.Id.ToArray());
                var reply = new GetBoardsReply();

                reply.Boards.AddRange(response.Select(b => new BoardInfo { Id = b.Id, Title = b.Title, Type = (int)b.Type }));
                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<UpdateBoardReply> UpdateBoard(UpdateBoardRequest request, ServerCallContext context)
        {
            var boardModel = BoardModel.Create(request.Title, id: request.Id);

            if (boardModel.IsError)
            {
                logger.LogError(boardModel.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, boardModel.StringErrors));
            }

            try
            {
                var reply = await repository.UpdateAsync(boardModel.Value);
                return new UpdateBoardReply { Id = reply };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }

        }
    }
}
