using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Common;
using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Services
{
    public class AuthService(IAuthDataRepository userRepository, IPasswordHasher passwordHasher) : IAuthService
    {
        public Result<Guid> Register(AuthDTO userData)
        {
            var resultFactory = new ResultFactory<Guid>();
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
                resultFactory.SetResult(userId);


                //verify

                return resultFactory.Create();
            }
            catch (Exception ex)
            {
                resultFactory.AddError(ex.Message);
                return resultFactory.Create();
            }
        }

        public Result<Guid?> Login(AuthDTO authData)
        {
            var resultFactory = new ResultFactory<Guid?>();
            var user = userRepository.GetByEmail(authData.Email).Result;

            if (user == null)
            {
                resultFactory.AddError("Пользователь не найден");
                return resultFactory.Create();
            }

            if (!passwordHasher.VerifyPassword(authData.Password, user.PasswordHash))
            {
                resultFactory.AddError("Неверный пароль");
                return resultFactory.Create();
            }

            resultFactory.SetResult(user.UserId);
            return resultFactory.Create();
        }
    }
}
