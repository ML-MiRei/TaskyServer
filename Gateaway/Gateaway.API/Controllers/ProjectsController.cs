using Gateaway.Core.Common;
using Gateaway.Core.ReplyModels;
using Gateaway.Core.RequestModels.Projects;
using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.Members;
using Getaway.Core.Contracts.ProjectBoards;
using Getaway.Core.Contracts.Projects;
using Getaway.Core.Contracts.Users;
using Getaway.Core.Enums;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController(Connections connections) : ControllerBase
    {

        [HttpGet("all")]
        public async Task<ActionResult<List<ProjectModel>>> GetListProjects()
        {
            try
            {
                //warning
                var userId = HttpContext.User?.FindFirst("userId")?.Value;
                List<Task> tasks = new List<Task>();

                List<ProjectModel> response = new List<ProjectModel>();

                var projectsReply = await connections.ProjectServiceClient.GetAllProjectsAsync(new GetAllProjectsRequest { UserId = userId });

                foreach (var project in projectsReply.Projects)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var boards = (await connections.ProjectBoardsServiceClient.GetAllBoardsAsync(new GetAllBoardsRequest { ProjectId = project.Id })).Boards.Select(b => b.BoardId).ToArray();
                        var members = (await connections.MembersServiceClient.GetMembersAsync(new GetMembersRequest { ProjectId = project.Id })).Members.Select(m => new MemberModel
                        {
                            UserId = m.UserId,
                            Role = new RoleModel { Id = m.Role.RoleId, Name = m.Role.Name }
                        }).ToArray();

                        response.Add(new ProjectModel
                        {
                            Id = project.Id,
                            Title = project.Title,
                            Details = project.Details,
                            BoardIds = boards,
                            Members = members
                        });

                    }));
                }

                Task.WaitAll(tasks.ToArray());

                return Ok(response);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectModel>> GetProject(string projectId)
        {
            try
            {
                var userId = HttpContext.User?.FindFirst("userId")?.Value;
                var project = await connections.ProjectServiceClient.GetProjectAsync(new GetProjectRequest { ProjectId = projectId });

                var boards = connections.ProjectBoardsServiceClient.GetAllBoards(new GetAllBoardsRequest { ProjectId = projectId }).Boards.Select(b => b.BoardId).ToArray();
                var members = connections.MembersServiceClient.GetMembers(new GetMembersRequest { ProjectId = projectId }).Members.Select(m => new MemberModel
                {
                    UserId = m.UserId,
                    Role = new RoleModel { Id = m.Role.RoleId, Name = m.Role.Name }
                }).ToArray();

                var role = members.First(m => m.UserId == userId).Role.Name;

                (HttpContext.User.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

                var response = new ProjectModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    Details = project.Details,
                    BoardIds = boards,
                    Members = members
                };

                return Ok(project);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = 404, Content = ex.Message };
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateProject([FromBody] Gateaway.Core.RequestModels.Projects.CreateProjectRequest createProjectRequest)
        {
            try
            {
                var userId = HttpContext.User?.FindFirst("userId")?.Value;
                var request = new Core.Contracts.Projects.CreateProjectRequest() { UserId = userId, Details = createProjectRequest.Details, Title = createProjectRequest.Title };

                var reply = await connections.ProjectServiceClient.CreateProjectAsync(request);
                (HttpContext.User.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));

                return Ok(reply.ProjectId);
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.ToString());
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{projectId}")]
        // [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> DeleteProject(string projectId)
        {
            try
            {
                var reply = await connections.ProjectServiceClient.DeleteProjectAsync(new DeleteProjectRequest { ProjectId = projectId });
                return Ok(reply.ProjectId);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{projectId}")]
        //  [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> UpdateProject(string projectId, [FromBody] Gateaway.Core.RequestModels.Projects.UpdateProjectRequest updateProjectRequest)
        {
            try
            {
                var reply = await connections.ProjectServiceClient.UpdateProjectAsync(new Core.Contracts.Projects.UpdateProjectRequest
                {
                    Id = projectId,
                    Title = updateProjectRequest.Title,
                    Details = updateProjectRequest.Details
                });
                return Ok(reply.ProjectId);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("{projectId}/members")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> AddMember(string projectId, [FromBody] Gateaway.Core.RequestModels.Projects.AddMemberRequest addMemberRequest)
        {
            try
            {
                var reply = await connections.MembersServiceClient.AddMemberAsync(new Core.Contracts.Members.AddMemberRequest { ProjectId = projectId, UserId = addMemberRequest.UserId });
                return Ok(reply.ProjectId);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("{projectId}/members/{memberId}")]
        public async Task<ActionResult<string>> DeleteMember(string projectId, string memberId)
        {
            try
            {
                var userId = HttpContext.User?.FindFirst("userId")?.Value;

                if (!HttpContext.User.IsInRole("admin") && userId != memberId)
                    return StatusCode(403);
                else if (HttpContext.User.IsInRole("admin") && userId == memberId)
                    return BadRequest();

                var reply = await connections.MembersServiceClient.DeleteMemberAsync(new DeleteMemberRequest { UserId = memberId, ProjectId = projectId });
                return Ok(reply.ProjectId);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{projectId}/members/{memberId}/role")]
        // [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> ChangeRoleMember(string projectId, string memberId, [FromBody] Gateaway.Core.RequestModels.Projects.ChangeMemberRoleRequest changeMemberRoleRequest)
        {
            try
            {
                var reply = await connections.MembersServiceClient.ChangeMemberRoleAsync(new Core.Contracts.Members.ChangeMemberRoleRequest
                {
                    ProjectId = projectId,
                    UserId = memberId,
                    RoleId = changeMemberRoleRequest.RoleId
                });
                return Ok(reply.ProjectId);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        //delete
        [HttpGet("{projectId}/members")]
        public async Task<ActionResult<List<UserModel>>> GetMembers(string projectId)
        {
            try
            {
                var replyMembers = await connections.MembersServiceClient.GetMembersAsync(new GetMembersRequest { ProjectId = projectId });

                var request = new GetUsersRequest();
                request.Id.AddRange(replyMembers.Members.Select(m => m.UserId));

                var replyUsers = await connections.UsersServiceClient.GetUsersAsync(request);

                var members = replyUsers.Users.Join(replyMembers.Members, m => m.Id, u => u.UserId, (m, u) =>
                {
                    return new UserModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Picture = m.ProfilePicture.ToString(),    ///error warning
                        ProjectRole = (ProjectMemberRoles)u.Role.RoleId
                    };
                });

                return Ok(members);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        //delete
        [HttpGet("{projectId}/boards")]
        public async Task<ActionResult<List<Core.Contracts.Boards.BoardInfo>>> GetBoards(string projectId)
        {
            try
            {
                var replyBoardsIds = await connections.ProjectBoardsServiceClient.GetAllBoardsAsync(new GetAllBoardsRequest { ProjectId = projectId });

                var request = new GetBoardsRequest();
                request.Id.AddRange(replyBoardsIds.Boards.Select(m => m.BoardId));

                var replyBoards = await connections.BoardsServiceClient.GetBoardsAsync(request);

                return Ok(replyBoards.Boards);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("{projectId}/boards")]
        public async Task<ActionResult<string>> AddBoard(string projectId, [FromBody] Gateaway.Core.RequestModels.Projects.AddBoardRequest addBoardRequest)
        {
            try
            {
                var reply = await connections.ProjectBoardsServiceClient.AddBoardAsync(new Core.Contracts.ProjectBoards.AddBoardRequest
                {
                    ProjectId = projectId,
                    BoardId = addBoardRequest.BoardId
                });
                return Ok(reply.BoardId);
            }
            catch (RpcException ex)
            {
                return new ContentResult() { StatusCode = (int)ex.StatusCode, Content = ex.Message };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}
