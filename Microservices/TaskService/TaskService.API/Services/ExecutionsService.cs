using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TaskService.Application.Abstractions.Repositories;
using TaskService.Application.Services;
using TaskService.Core.Common;
using TaskService.Core.Models;

namespace TaskService.API.Services
{
    public class ExecutionsService(ILogger<ExecutionsService> logger, IExecutionsRepository executionsRepository, ExecutionManager executionManager) : Executions.ExecutionsBase
    {
        public override async Task<AddExecutorReply> AddExecutor(AddExecutorRequest request, ServerCallContext context)
        {
            var execution = new ExecutionModel(request.TaskId, request.ExecutorId);

            try
            {
                var res = await executionManager.AddExecutor(execution);
                return new AddExecutorReply { TaskId = res };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<DeleteExecutorReply> DeleteExecutor(DeleteExecutorRequest request, ServerCallContext context)
        {
            var execution = new ExecutionModel(request.TaskId, request.ExecutorId);

            try
            {
                var res = await executionManager.DeleteExecutor(execution);
                return new DeleteExecutorReply { TaskId = res };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public override async Task<GetExecutorsReply> GetExecutors(GetExecutorsRequest request, ServerCallContext context)
        {
            try
            {
                var res = await executionsRepository.GetExecutorsByTaskIdAsync(request.TaskId);
                var reply = new GetExecutorsReply();

                reply.ExecutorIds.AddRange(res.Select(e => e.UserId));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetHistoryByTaskReply> GetHistoryByTask(GetHistoryByTaskRequest request, ServerCallContext context)
        {
            try
            {
                var res = await executionsRepository.GetHistoryExecutionsByTaskIdAsync(request.TaskId);
                var reply = new GetHistoryByTaskReply();

                reply.Executions.AddRange(res.Select(e => new ExecutionInfo
                {
                    TaskId = e.TaskId,
                    DateCreated = Timestamp.FromDateTimeOffset(e.DateStart),
                    ExecutorId = e.UserId,
                    Status = (int)e.Status
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetHistoryByUserReply> GetHistoryByUser(GetHistoryByUserRequest request, ServerCallContext context)
        {
            try
            {
                var res = await executionsRepository.GetHistoryExecutionsByUserIdAsync(request.ExecutorId);
                var reply = new GetHistoryByUserReply();

                reply.Executions.AddRange(res.Select(e => new ExecutionInfo
                {
                    TaskId = e.TaskId,
                    DateCreated = Timestamp.FromDateTimeOffset(e.DateStart),
                    ExecutorId = e.UserId,
                    Status = (int)e.Status
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public async override Task<GetStateExecutionsByUserReply> GetStateExecutionsByUser(GetStateExecutionsByUserRequest request, ServerCallContext context)
        {
            try
            {
                var res = await executionsRepository.GetStateExecutionsByUserIdAsync(request.ExecutorId);
                var reply = new GetStateExecutionsByUserReply();

                reply.Executions.AddRange(res.Select(e => new ExecutionInfo
                {
                    TaskId = e.TaskId,
                    DateCreated = Timestamp.FromDateTimeOffset(e.DateStart),
                    ExecutorId = e.UserId,
                    Status = (int)e.Status
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public override async Task<SetFinishedExecutionsReply> SetFinishedExecutions(SetFinishedExecutionsRequest request, ServerCallContext context)
        {
            try
            {
                var res = await executionManager.SetFinishedExecutions(request.TaskId);
                return new SetFinishedExecutionsReply { TaskId = res };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }
    }
}
