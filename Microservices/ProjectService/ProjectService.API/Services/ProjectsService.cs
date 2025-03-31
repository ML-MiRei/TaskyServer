using Grpc.Core;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Common;
using ProjectService.Core.Enums;
using ProjectService.Core.Models;

namespace ProjectService.API.Services
{
    public class ProjectsService(ILogger<ProjectsService> logger,
                                 IProjectsRepository projectsRepository,
                                 IMembersRepository membersRepository) : Projects.ProjectsBase
    {

        public override async Task<CreateProjectReply> CreateProject(CreateProjectRequest request, ServerCallContext context)
        {
            var projectRes = ProjectModel.Create(request.Title, request.Details);

            if (projectRes.IsError)
            {
                logger.LogDebug(projectRes.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, projectRes.StringErrors));
            }

            try
            {
                var newProject = projectRes.Value;
                var projectId = await projectsRepository.CreateAsync(newProject);

                Console.WriteLine("id = " + projectId);

                await membersRepository.AddAsync(new MemberModel(request.UserId, projectId, new RoleModel((int)MemberRoles.Admin, MemberRoles.Admin.ToString())));
                return new CreateProjectReply { ProjectId = newProject.Id.ToString() };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));
            }
        }

        public async override Task<DeleteProjectReply> DeleteProject(DeleteProjectRequest request, ServerCallContext context)
        {
            try
            {
                var deletedProjectId = await projectsRepository.DeleteAsync(request.ProjectId);

                // notificate / +- kafka

                return new DeleteProjectReply { ProjectId = deletedProjectId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.DELETE_ERROR_MESSAGE));

            }
        }

        public async override Task<GetAllProjectsReply> GetAllProjects(GetAllProjectsRequest request, ServerCallContext context)
        {
            try
            {
                var projects = await projectsRepository.GetAllAsync(request.UserId);
                var reply = new GetAllProjectsReply();

                reply.Projects.AddRange(projects.Select(p => new ProjectShortInfo { Id = p.Id, Title = p.Title, Details = p.Details }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }
        }

        public override async Task<GetProjectReply> GetProject(GetProjectRequest request, ServerCallContext context)
        {
            try
            {
                var project = await projectsRepository.GetByIdAsync(request.ProjectId);

                if (project != null)
                    return new GetProjectReply { Details = project.Details, Title = project.Title, Id = project.Id };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.INTERNAL_ERROR_MESSAGE));
            }

            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
        }

        public async override Task<UpdateProjectReply> UpdateProject(UpdateProjectRequest request, ServerCallContext context)
        {
            var projectRes = ProjectModel.Create(request.Title, request.Details, request.Id);

            if (projectRes.IsError)
            {
                logger.LogDebug(projectRes.StringErrors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, projectRes.StringErrors));
            }

            try
            {
                var newProject = projectRes.Value;
                var projectId = await projectsRepository.UpdateAsync(newProject);

                // notificate / +- kafka

                if (projectId != null)
                    return new UpdateProjectReply { ProjectId = newProject.Id };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessagesConsts.SAVE_ERROR_MESSAGE));

            }

            throw new RpcException(new Status(StatusCode.NotFound, ErrorMessagesConsts.NOT_FOUND_ERROR_MESSAGE));
        }
    }
}
