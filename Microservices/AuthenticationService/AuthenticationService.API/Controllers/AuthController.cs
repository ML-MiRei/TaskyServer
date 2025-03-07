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
        public IResult Registration(AuthDTO userData)
        {
            var result = authService.Register(userData);

            if (result.IsError)
                return Results.Problem(string.Join("; ", result.Errors));


            HttpContext.Response.Cookies.Append("atj-user", result.Value);
            return Results.Ok(result.Value);
        }

        [HttpPatch("/login")]
        public IResult Login(AuthDTO authData)
        {
            var result = authService.Login(authData);
            if (result.IsError)
                return Results.Problem(string.Join("; ", result.Errors));

            HttpContext.Response.Cookies.Append("atj-user", result.Value);
            return Results.Ok(result.Value);

        }


    }
}
