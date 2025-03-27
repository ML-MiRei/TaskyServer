using CommentService.API;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Application.Services;
using TaskService.Core.Common;
using TaskService.Core.Models;

namespace TaskService.API.Services
{
    public class CommentsService(ILogger<CommentsService> logger, CommentsManager commentsManager, ICommentsRepository commentsRepository) : Comments.CommentsBase
    {
        public override async Task<CreateCommentReply> CreateComment(CreateCommentRequest request, ServerCallContext context)
        {
            var comment = CommentModel.Create(request.CreatorId, request.Text, request.TaskId);

            if (comment.IsError)
            {
                logger.LogDebug(comment.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, comment.StringErrors));
            }

            try
            {
                var commentId = await commentsRepository.CreateAsync(comment.Value);
                return new CreateCommentReply { CommentId = commentId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }

        }

        public override async Task<DeleteCommentReply> DeleteComment(DeleteCommentRequest request, ServerCallContext context)
        {
            try
            {
                var commentId = await commentsManager.DeleteComment(request.CommentId);

                if (commentId)
                    return new DeleteCommentReply { CommentId = request.CommentId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }

            throw new RpcException(new Status(StatusCode.Unavailable, "Нельзя удалить комментарий, написанный более чем 3 минуты назад"));
        }

        public async override Task<GetAllCommentsByTaskReply> GetAllCommentsByTask(GetAllCommentsByTaskRequest request, ServerCallContext context)
        {
            try
            {
                var comments = await commentsRepository.GetAllByTaskAsync(request.TaskId);
                var reply = new GetAllCommentsByTaskReply();

                reply.Comments.AddRange(comments.Select(c => new CommentInfo
                {
                    CommentId = c.Id.Value,
                    CreatorId = c.CreatorId,
                    DateCreated = Timestamp.FromDateTimeOffset(c.Created),
                    TaskId = c.TaskId,
                    Text = c.Text
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<UpdateCommentReply> UpdateComment(UpdateCommentRequest request, ServerCallContext context)
        {
            var comment = CommentModel.Create(request.CreatorId, request.Text);

            if (comment.IsError)
            {
                logger.LogDebug(comment.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, comment.StringErrors));
            }

            try
            {
                var res = await commentsManager.UpdateComment(comment.Value);

                if (res)
                    return new UpdateCommentReply { CommentId = request.CommentId};
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }

            throw new RpcException(new Status(StatusCode.Unavailable, "Нельзя редактировать комментарий, написанный более чем 3 минуты назад"));
        }
    }
}
