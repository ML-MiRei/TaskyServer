using Grpc.Core;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Application.Abstractions.Services;
using ProjectService.Core.Common;
using ProjectService.Core.Enums;
using ProjectService.Core.Models;

namespace ProjectService.API.Services
{
    public class MembersService(IMembersRepository membersRepository, MembersManager membersManager, ILogger<MembersService> logger) : Members.MembersBase
    {
        public async override Task<AddMemberReply> AddMember(AddMemberRequest request, ServerCallContext context)
        {
            try
            {
                var user = await membersManager.AddMemberAsync(new MemberModel(request.UserId, request.ProjectId));

                return new AddMemberReply { UserId = request.UserId, ProjectId = request.ProjectId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<ChangeMemberRoleReply> ChangeMemberRole(ChangeMemberRoleRequest request, ServerCallContext context)
        {
            try
            {
                var user = await membersManager.ChangeMemberRoleAsync(new MemberModel(request.UserId, request.ProjectId, new RoleModel(request.RoleId, "")));
                return new ChangeMemberRoleReply { UserId = request.UserId, ProjectId = request.ProjectId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }

        }

        public async override Task<DeleteMemberRequest> DeleteMember(DeleteMemberRequest request, ServerCallContext context)
        {

            try
            {
                var deletedUser = await membersManager.DeleteMemberAsync(new MemberModel(request.UserId, request.ProjectId));
                return new DeleteMemberRequest { ProjectId = request.ProjectId, UserId = request.UserId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));
            }
        }

        public async override Task<GetMembersReply> GetMembers(GetMembersRequest request, ServerCallContext context)
        {
            try
            {
                var members = await membersRepository.GetAllAsync(request.ProjectId);
                var res = new GetMembersReply();

                res.Members.AddRange(members.Select(m => new MemberInfo { UserId = m.Id.ToString(), Role = new RoleInfo { RoleId = m.Role.Id, Name = m.Role.Name } }));

                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public override async Task<GetMemberRoleReply> GetMemberRole(GetMemberRoleRequest request, ServerCallContext context)
        {
            try
            {
                var role = await membersRepository.GetRoleAsync(new MemberModel(request.UserId, request.ProjectId));
                return new GetMemberRoleReply { RoleId = role.Id, Name = role.Name };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }
    }
}
