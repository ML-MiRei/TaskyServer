using Gateaway.Core.Common;
using Gateaway.Core.ReplyModels;
using Getaway.Core.Contracts.ProjectBoards;
using Getaway.Core.Contracts.Projects;
using Getaway.Core.Contracts.Tasks;
using Getaway.Core.Enums;

namespace Gateaway.Application.Services
{
    public class ProjectsService
    {
        //public async List<ProjectModel> GetListProjects(string userId)
        //{
        //    var reply = await Connections.ProjectServiceClient.GetAllProjectsAsync(new GetAllProjectsRequest { UserId = userId });
        //    var projectModels = reply.Projects.Select(p => new ProjectModel { Id = p.Id, Title = p.Title, Details = p.Details });

        //    return projectModels;
        //}

        //public async List<ProjectModel> GetProject(string projectId)
        //{
        //    var projectInfoReply = Connections.ProjectServiceClient.GetProjectAsync(new GetProjectRequest { ProjectId = projectId });
        //    var projectBoardsReply = Connections.ProjectBoardsServiceClient.GetAllBoardsAsync(new GetAllBoardsRequest { ProjectId = projectId });
        //    var tasksReply = Connections.TasksServiceClient.GetAllTasksByProjectAsync(new GetAllTasksByProjectRequest { ProjectId = projectId });

        //    var projectBoards = await projectBoardsReply;

        //    var getBoardsRequest = new Getaway.Core.Contracts.Boards.GetBoardsRequest();
        //    getBoardsRequest.Id.AddRange(projectBoards.Boards.Select(b => b.BoardId));

        //    var boardsReply = Connections.BoardsServiceClient.GetBoardsAsync(getBoardsRequest);

        //    var tasks = (await tasksReply).Tasks;
        //    var boards = (await boardsReply).Boards;
        //    var project = await projectInfoReply;

        //    ProjectModel projectModel = new ProjectModel
        //    {
        //        Id = projectId,
        //        Details = project.Details,
        //        Title = project.Title,
        //        Boards = boards.Select(b => new BoardModel
        //        {
        //            Id = b.Id,
        //            Title = b.Title,
        //            Type = (BoardType)b.Type
        //        }).ToList(),
        //        Tasks = tasks.Select(t => new TaskModel
        //        {
        //            Id = t.TaskId,
        //            Title = t.Title,
        //            Details = t.Details
        //        })
        //    } 
        //}
    }
}
