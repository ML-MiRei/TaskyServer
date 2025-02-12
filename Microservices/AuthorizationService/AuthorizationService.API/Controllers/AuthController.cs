using AuthorizationService.Applicaion.Abstractions.Services;
using AuthorizationService.Applicaion.DTO;
using AuthorizationService.Applicaion.Services;
using AuthorizationService.Core.Models;
using Microsoft.AspNetCore.Mvc;


namespace AuthorizationService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("/reg")]
        public IResult Registration(UserDTO userData)
        {
            var result = authService.Register(userData);

            if (result.IsSuccess)
                return Results.Ok(result.Value);
            else
                return Results.Problem(string.Join("; ", result.Errors));

        }

        [HttpPatch("/login")]
        public IResult Login(AuthDTO authData)
        {
            var result = authService.Login(authData);
            if (result.IsSuccess)
                return Results.Ok(result.Value);
            else
                return Results.Problem(string.Join("; ", result.Errors));
        }


    }
}
