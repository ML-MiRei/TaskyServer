using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Applicaion.Services;
using AuthenticationService.Core.Models;
using Microsoft.AspNetCore.Mvc;


namespace AuthenticationService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
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
