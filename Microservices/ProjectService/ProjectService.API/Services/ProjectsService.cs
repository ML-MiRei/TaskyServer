using Grpc.Core;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Models;

namespace ProjectService.API.Services
{
    public class ProjectsService(ILogger<ProjectsService> logger,
                                 IProjectsRepository projectsRepository,
                                 IMembersRepository membersRepository,
                                 IProjectTasksRepository projectTasksRepository) : Projects.ProjectsBase
    {

        public override async Task<CreateProjectReply> CreateProject(CreateProjectRequest request, ServerCallContext context)
        {
            var projectRes = ProjectModel.Create(request.Name, description: request.Description);
            if (!projectRes.IsSuccess)
            {
                logger.LogDebug($"Project is not created, errors: {string.Join(',', projectRes.Errors)}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(',', projectRes.Errors)));
            }

            try
            {
                var newProject = projectRes.Value;
                var projectId = await projectsRepository.CreateAsync(newProject);
                logger.LogDebug($"Project {projectId} created");

                // change role id on const

                await membersRepository.AddAsync(newProject.Id, new MemberModel(Guid.Parse(request.UserId), 1));
                return new CreateProjectReply { ProjectId = newProject.Id.ToString() };

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка сохранения. Повторите попытку позже"));
            }
        }

        public async override Task<DeleteProjectReply> DeleteProject(DeleteProjectRequest request, ServerCallContext context)
        {

            try
            {
                var deletedProjectId = await projectsRepository.DeleteAsync(Guid.Parse(request.ProjectId));

                // notificate / +- kafka

                return new DeleteProjectReply { ProjectId = deletedProjectId.ToString() };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка удаления. Повторите попытку позже"));

            }

        }

        public async override Task<GetAllProjectsReply> GetAllProjects(GetAllProjectsRequest request, ServerCallContext context)
        {

            try
            {
                var models = await projectsRepository.GetAllAsync(Guid.Parse(request.UserId));
                ProjectShortInfo projectShortInfo = new ProjectShortInfo();
                var projects = new GetAllProjectsReply();

                projects.Projects.AddRange(models.Select(m =>
                {
                    Task<int> count = Task.Run(() => projectTasksRepository.GetSuccessAsync(m.Id));
                    Task<int> countDone = Task.Run(() => projectTasksRepository.GetCountAsync(m.Id));

                    Task.WaitAll(count, countDone);

                    return new ProjectShortInfo
                    {
                        Id = m.Id.ToString(),
                        Name = m.Name,
                        ProcentDoneTasks = Math.Round(Convert.ToDouble(count.Result / 100 * countDone.Result), 1),
                        CountTasks = count.Result
                    };
                }));

                return projects;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения данных. Повторите попытку позже"));

            }
        }

        public override async Task<GetProjectReply> GetProject(GetProjectRequest request, ServerCallContext context)
        {
            try
            {
                var model = await projectsRepository.GetByIdAsync(Guid.Parse(request.ProjectId));
                return new GetProjectReply { Description = model.Description, Name = model.Name, Id = model.Id.ToString() };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения данных. Повторите попытку позже"));
            }

        }

        public async override Task<UpdateProjectReply> UpdateProject(UpdateProjectRequest request, ServerCallContext context)
        {

            var projectRes = ProjectModel.Create(request.Name, description: request.Description);
            if (!projectRes.IsSuccess)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(',', projectRes.Errors)));
            }

            try
            {
                var newProject = projectRes.Value;
                var projectId = await projectsRepository.UpdateAsync(newProject);

                // notificate / +- kafka

                return new UpdateProjectReply { ProjectId = newProject.Id.ToString() };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка сохранения. Повторите попытку позже"));

            }
        }
    }
}
