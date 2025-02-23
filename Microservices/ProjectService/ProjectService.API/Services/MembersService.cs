using Grpc.Core;
using ProjectService.API;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;

namespace ProjectService.API.Services
{
    public class MembersService(IMembersRepository membersRepository, ILogger<MembersService> logger) : Members.MembersBase
    {
        public async override Task<AddMemberReply> AddMember(AddMemberRequest request, ServerCallContext context)
        {

            try
            {
                var user = await membersRepository.AddAsync(Guid.Parse(request.ProjectId), new MemberModel( Guid.Parse(request.UserId), 0)); // change on const

                // notificate / +- kafka
                return new AddMemberReply { UserId = request.UserId, ProjectId = request.ProjectId };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка добаввления пользователя. Повторите попытку позже"));
            }
        }

        public async override Task<ChangeMemberRoleReply> ChangeMemberRole(ChangeMemberRoleRequest request, ServerCallContext context)
        {

            try
            {
                var user = await membersRepository.UpdateRoleAsync(Guid.Parse(request.ProjectId), Guid.Parse(request.UserId), request.UserRole);

                // notificate / +- kafka
                return new ChangeMemberRoleReply { UserId = request.UserId, ProjectId= request.ProjectId };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка обновления данных. Повторите попытку позже"));

            }

        }

        public async override Task<DeleteMemberRequest> DeleteMember(DeleteMemberRequest request, ServerCallContext context)
        {

            try
            {
                var deletedUser = await membersRepository.DeleteAsync(Guid.Parse(request.ProjectId), Guid.Parse(request.UserId));
                // notificate / +- kafka

                return new DeleteMemberRequest { ProjectId = request.ProjectId, UserId = request.UserId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка удаления пользователя. Повторите попытку позже"));

            }

        }

        public async override Task<GetMembersReply> GetMembers(GetMembersRequest request, ServerCallContext context)
        {

            try
            {
                var members = await membersRepository.GetAllAsync(Guid.Parse(request.ProjectId));

                // request in user service

                var res = new GetMembersReply();
                res.Members.AddRange(members.Select(m => new MemberInfo { Name = m.Name, PicturePath = m.PicturePath, UserId = m.Id.ToString() }));
                // cash
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения. Повторите попытку позже"));

            }

        }
    }
}
