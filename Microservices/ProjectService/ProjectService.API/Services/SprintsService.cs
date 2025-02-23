using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;
using SprintService.API;

namespace ProjectService.API.Services
{
    public class SprintsService(ILogger<SprintsService> logger, ISprintsRepository sprintTasksRepository) : Sprints.SprintsBase
    {
        public override async Task<CreateSprintReply> CreateSprint(CreateSprintRequest request, ServerCallContext context)
        {

            var sprintRes = SprintModel.Create(request.DateStart.ToDateTime(), request.DateEnd.ToDateTime(), project: Guid.Parse(request.ProjectId));

            if (!sprintRes.IsSuccess)
            {
                logger.LogDebug($"Sprint is not created, errors: {string.Join(',', sprintRes.Errors)}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(',', sprintRes.Errors)));
            }

            try
            {
                var newSprint = sprintRes.Value;
                var sprintId = await sprintTasksRepository.CreateAsync(newSprint);
                
                return new CreateSprintReply { SprintId = sprintId };  
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка сохранения. Повторите попытку позже"));
            }

        }

        public override async Task<DeleteSprintReply> DeleteSprint(DeleteSprintRequest request, ServerCallContext context)
        {

            try
            {
                var deletedSprintId = await sprintTasksRepository.DeleteAsync(request.SprintId);
                return new DeleteSprintReply { SprintId = deletedSprintId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка удаления. Повторите попытку позже"));

            }

        }

        public override async Task<GetAllSprintsReply> GetAllSprints(GetAllSprintsRequest request, ServerCallContext context)
        {

            try
            {
                var sprints = await sprintTasksRepository.GetAllAsync(Guid.Parse(request.ProjectId));
                var res = new GetAllSprintsReply();

                res.Sprints.AddRange(sprints.Select(s => new SprintInfo { DateEnd = s.DateEnd.ToTimestamp(), DateStart = s.DateStart.ToTimestamp(), Id = s.Id.Value }));

                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения данных. Повторите попытку позже"));

            }

        }

        public override async Task<UpdateSprintReply> UpdateSprint(UpdateSprintRequest request, ServerCallContext context)
        {

            var sprintRes = SprintModel.Create(request.DateStart.ToDateTime(), request.DateEnd.ToDateTime(), request.SprintId);


            if (!sprintRes.IsSuccess)
            {
                logger.LogDebug($"Task is not updated, errors: {string.Join(',', sprintRes.Errors)}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(',', sprintRes.Errors)));
            }

            try
            {
                var newSprint = sprintRes.Value;
                var sprintId = await sprintTasksRepository.UpdateAsync(newSprint);
                
                return new UpdateSprintReply { SprintId =  sprintId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка сохранения. Повторите попытку позже"));
            }

        }
    }
}
