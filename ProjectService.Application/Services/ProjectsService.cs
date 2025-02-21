using Microsoft.Extensions.Logging;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Application.Common.Base;
using ProjectService.Application.DTOs;
using ProjectService.Core.Common;
using ProjectService.Core.Models;

namespace ProjectService.Application.Services
{
    public class ProjectsService(ILogger<ProjectsService> logger, 
                                 IProjectsRepository projectsRepository,
                                 IMembersRepository membersRepository
        ): GetCrudCommandsBase<ProjectModel, Guid, IProjectsRepository>(projectsRepository, logger)
    {

        public async Task<Result<ProjectModel>> CreateAsync(Guid userId, ProjectDTO projectDTO)
        {
            var res = new ResultFactory<ProjectModel>();

            var projectRes = ProjectModel.Create(projectDTO.Name, description: projectDTO.Description);
            if (!projectRes.IsSuccess)
            {
                res.AddError(projectRes.Errors.ToArray());
                return res.Create();
            }

            try
            {
                var newProject = projectRes.Value;
                var projectId = await projectsRepository.CreateAsync(newProject);

                // change role id on const

                await membersRepository.AddAsync(newProject.Id, new MemberModel(userId, 1));

                res.SetResult(newProject);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка сохранения. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<ProjectModel>> UpdateAsync(ProjectDTO projectDTO)
        {
            var res = new ResultFactory<ProjectModel>();

            var projectRes = ProjectModel.Create(projectDTO.Name, description: projectDTO.Description);
            if (!projectRes.IsSuccess)
            {
                res.AddError(projectRes.Errors.ToArray());
                return res.Create();
            }

            try
            {
                var newProject = projectRes.Value;
                var projectId = await projectsRepository.UpdateAsync(newProject);

                //add user

                res.SetResult(newProject);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка сохранения. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<Guid>> DeleteAsync(Guid projectId)
        {
            var res = new ResultFactory<Guid>();

            try
            {
                var deletedProjectIId = await projectsRepository.DeleteAsync(projectId);
                res.SetResult(deletedProjectIId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка удаления. Повторите попытку позже");
            }

            return res.Create();
        }


       

    }
}
