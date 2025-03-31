using BoardService.API;
using Grpc.Core;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Common;
using ProjectService.Core.Models;

namespace ProjectService.API.Services
{
    public class BoardsService(ILogger<BoardsService> logger, IBoardsRepository boardsRepository) : Boards.BoardsBase
    {
        public override async Task<AddBoardReply> AddBoard(AddBoardRequest request, ServerCallContext context)
        {
            try
            {
                var boardId = await boardsRepository.CreateAsync(new BoardModel(request.BoardId, request.ProjectId));

                return new AddBoardReply { BoardId = boardId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public override async Task<DeleteBoardReply> DeleteBoard(DeleteBoardRequest request, ServerCallContext context)
        {
            try
            {
                var boardId = await boardsRepository.DeleteAsync(new BoardModel(request.BoardId, request.ProjectId));

                return new DeleteBoardReply { BoardId = boardId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public override async Task<GetAllBoardsReply> GetAllBoards(GetAllBoardsRequest request, ServerCallContext context)
        {
            try
            {
                var sprints = await boardsRepository.GetByProjectAsync(request.ProjectId);
                var reply = new GetAllBoardsReply();

                reply.Boards.AddRange(sprints.Select(b => new BoardInfo { BoardId = b.BoardId, ProjectId = b.ProjectId}));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }
    }
}
