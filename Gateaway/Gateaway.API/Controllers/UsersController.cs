using Gateaway.Core.Common;
using Getaway.Core.Contracts.Users;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Getaway.Presentation.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController(Connections connections) : ControllerBase

    {

        [HttpGet]
        public async Task<ActionResult<List<UserShortInfo>>> GetListUsers([FromBody] GetUsersRequest request)
        {
            try
            {
                var reply = await connections.UsersServiceClient.GetUsersAsync(request);
                return Ok(reply.Users.ToList());
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

        [HttpGet("name={userName}")]
        public async Task<ActionResult<List<UserShortInfo>>> FindByNameUsers(string userName)
        {
            try
            {
                var reply = await connections.UsersServiceClient.FindByNameAsync(new FindByNameUsersRequest { Name = userName });
                return Ok(reply.Users.ToList());
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

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserFullInfo>> GetUser(string userId)
        {
            try
            {
                var reply = connections.UsersServiceClient.GetUserAsync(new GetUserRequest { Id = userId});
                return Ok(reply);
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


        [HttpPut]
        public async Task<ActionResult<string>> UpdateProject([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var reply = await connections.UsersServiceClient.UpdateUserAsync(updateUserRequest);
                return Ok(reply);
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

