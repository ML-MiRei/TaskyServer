using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Common;
using AuthenticationService.Core.Models;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Applicaion.Services
{
    public class AuthService(IAuthDataRepository userRepository, IVerificationService verificationService, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, ILogger<AuthService> logger) : IAuthService
    {
        public Result<string> Register(AuthDTO userData)
        {
            var resultFactory = new ResultFactory<string>();
            var user = userRepository.GetByEmail(userData.Email).Result;

            if (user != null)
            {
                resultFactory.AddError("Пользователь уже зарегистрирован");
                return resultFactory.Create();
            }

            var passwordHash = passwordHasher.HashPassword(userData.Password);

            var newUser = AuthDataModel.Create(
                    userData.Email,
                    passwordHash
                );

            if (newUser.IsError)
            {
                resultFactory.AddError(newUser.Errors.ToArray());
                return resultFactory.Create();
            }

            try
            {
                var userId = userRepository.Create(newUser.Value).Result;
                var token = jwtProvider.GenerateToken(AuthDataModel.Create(userData.Email, passwordHash, userId).Value);

                resultFactory.SetResult(token);

                verificationService.VerificateEmail(userData.Email);

                return resultFactory.Create();
            }
            catch (Exception ex)
            {

                logger.LogError(ex.ToString());
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);

                resultFactory.AddError(ex.Message);
                return resultFactory.Create();
            }
        }

        public Result<string?> Login(AuthDTO userData)
        {
            var resultFactory = new ResultFactory<string?>();
            var user = userRepository.GetByEmail(userData.Email).Result;

            if (user == null)
            {
                resultFactory.AddError("Пользователь не найден");
                return resultFactory.Create();
            }

            if (!passwordHasher.VerifyPassword(userData.Password, user.PasswordHash))
            {
                resultFactory.AddError("Неверный пароль");
                return resultFactory.Create();
            }

            var token = jwtProvider.GenerateToken(AuthDataModel.Create(user.Email, user.PasswordHash, user.UserId, user.IsVerified).Value);

            resultFactory.SetResult(token);
            return resultFactory.Create();
        }
    }
}
