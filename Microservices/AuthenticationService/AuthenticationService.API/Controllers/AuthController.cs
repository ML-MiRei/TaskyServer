using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Applicaion.Services;
using Microsoft.AspNetCore.Mvc;


namespace AuthenticationService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(AuthService authService, IVerificationService verificationService, ILogger<AuthController> logger) : ControllerBase
    {

        [HttpPost("reg")]
        public IResult Registration(AuthDTO userData)
        {
            var result = authService.Register(userData);

            if (result.IsError)
                return Results.BadRequest(string.Join("; ", result.Errors));


            HttpContext.Response.Cookies.Append("atj-user", result.Value);
            return Results.Ok(result.Value);
        }

        [HttpPost("login")]
        public IResult Login([FromBody]AuthDTO authData)
        {
            var result = authService.Login(authData);
            if (result.IsError)
            {
                logger.LogError(string.Join("; ", result.Errors));
                return Results.BadRequest(string.Join("; ", result.Errors));
            }

            HttpContext.Response.Cookies.Append("atj-user", result.Value);
            return Results.Ok(result.Value);
        }

        [HttpPost("send-verification-link")]
        public IResult SendVerificationLink([FromBody] string email)
        {
            verificationService.VerificateEmail(email);
            return Results.Ok();
        }


        [HttpPost("verify")]
        public IResult Verify([FromBody] VerifyDTO verifyData)
        {
            var isValid = verificationService.TryVerify(verifyData.UserId, verifyData.Token).Result;
            if (!isValid)
            {
                return Results.BadRequest("Invalid or expired verification token");
            }

            return Results.Ok("Email successfully verified");

        }

    }
}
