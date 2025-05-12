using Gateaway.Core.Common;
using Gateaway.Core.ReplyModels;
using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.Members;
using Getaway.Core.Contracts.ProjectBoards;
using Getaway.Core.Contracts.Projects;
using Getaway.Core.Contracts.Users;
using Getaway.Core.Enums;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Core.Models;
using System.Data;
using System.Security.Claims;

namespace Getaway.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProjectController(Connections connections) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<ProjectShortInfo>>> Test()
        {
            HttpClient client = new HttpClient();

            //var res = await client.GetAsync("https://localhost:32774/test");
            return Ok("кк");
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ProjectShortInfo>>> GetListProjects()
        {
            try
            {
                //warning
                var userId = HttpContext.User?.FindFirst("userId")?.Value;

                var reply = await connections.ProjectServiceClient.GetAllProjectsAsync(new GetAllProjectsRequest { UserId = userId });
                return Ok(reply.Projects.ToList());
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
        public async Task<ActionResult<List<ProjectShortInfo>>> GetProject(string projectId)
        {
            try
            {
                var userId = HttpContext.User?.FindFirst("userId")?.Value;

                var phoneClaim = HttpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Role);
                (HttpContext.User.Identity as ClaimsIdentity).TryRemoveClaim(phoneClaim);

                var replyProject = connections.ProjectServiceClient.GetProjectAsync(new GetProjectRequest { ProjectId = projectId });
                var replyRole = connections.MembersServiceClient.GetMemberRoleAsync(new GetMemberRoleRequest { UserId = userId, ProjectId = projectId });

                var project = await replyProject;
                var role = await replyRole;

                (HttpContext.User.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));

                return Ok(project);
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

        [HttpPost]
        public async Task<ActionResult<string>> CreateProject([FromBody] CreateProjectRequest createProjectRequest)
        {
            try
            {
                var userId = HttpContext.User?.FindFirst("userId")?.Value;

                createProjectRequest.UserId = userId;

                var reply = await connections.ProjectServiceClient.CreateProjectAsync(createProjectRequest);
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
        [Authorize(Roles = "admin")]
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
        public async Task<ActionResult<string>> UpdateProject([FromBody] UpdateProjectRequest updateProjectRequest)
        {
            try
            {
                var reply = await connections.ProjectServiceClient.UpdateProjectAsync(updateProjectRequest);
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

        [HttpPost("{projectId}/add-member")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> AddMember([FromBody] AddMemberRequest addMemberRequest)
        {
            try
            {
                var reply = await connections.MembersServiceClient.AddMemberAsync(addMemberRequest);
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


        [HttpDelete("{projectId}/delete-member")]
        public async Task<ActionResult<string>> DeleteMember([FromBody] DeleteMemberRequest deleteMemberRequest)
        {
            try
            {
                var userId = HttpContext.User?.FindFirst("userId")?.Value;

                if (!HttpContext.User.IsInRole("admin") && userId != deleteMemberRequest.UserId)
                    return StatusCode(403);
                else if (HttpContext.User.IsInRole("admin") && userId == deleteMemberRequest.UserId)
                    return BadRequest();

                var reply = await connections.MembersServiceClient.DeleteMemberAsync(deleteMemberRequest);
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

        [HttpPut("{projectId}/change-member-role")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> ChangeRoleMember([FromBody] ChangeMemberRoleRequest changeMemberRoleRequest)
        {
            try
            {
                var reply = await connections.MembersServiceClient.ChangeMemberRoleAsync(changeMemberRoleRequest);
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

        [HttpGet("{projectId}/members")]
        public async Task<ActionResult<List<MemberModel>>> GetMembers(string projectId)
        {
            try
            {
                var replyMembers = await connections.MembersServiceClient.GetMembersAsync(new GetMembersRequest { ProjectId = projectId});

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

        [HttpGet("{projectId}/boards")]
        public async Task<ActionResult<List<Core.Contracts.Boards.BoardInfo>>> GetBoards(string projectId)
        {
            try
            {
                var replyBoardsIds = await connections.ProjectBoardsServiceClient.GetAllBoardsAsync(new GetAllBoardsRequest { ProjectId = projectId});

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

    }
}
