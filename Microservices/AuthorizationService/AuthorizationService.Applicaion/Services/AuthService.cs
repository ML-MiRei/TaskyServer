using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Applicaion.DTO;
using AuthenticationService.Core.Common;
using AuthenticationService.Core.Models;

namespace AuthenticationService.Applicaion.Services
{
    public class AuthService(IAuthDataRepository userRepository, IPasswordHasher passwordHasher) : IAuthService
    {
        public Result<Guid> Register(UserDTO userData)
        {
            var resultFactory = new ResultFactory<Guid>();
            var user = userRepository.GetByEmail(userData.Email);

            if (user != null)
            {
                resultFactory.AddError("Пользователь уже зарегистрирован");
                return resultFactory.Create();
            }

            var passwordHash = passwordHasher.HashPassword(userData.Password);

            var newUser = UserModel.Create(
                    userData.Id,
                    userData.Name,
                    userData.PhoneNumber,
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
                var userId = userRepository.Create(newUser.Value);
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

        public Result<UserModel> Login(AuthDTO authData)
        {
            var resultFactory = new ResultFactory<UserModel>();
            var user = userRepository.GetByEmail(authData.Email);

            if (user == null)
            {
                resultFactory.AddError("Пользователь не найден");
                return resultFactory.Create();
            }

            if (!passwordHasher.VerifyPassword(authData.Password, user.AuthData.PasswordHash))
            {
                resultFactory.AddError("Неверный пароль");
                return resultFactory.Create();
            }

            resultFactory.SetResult(user);
            return resultFactory.Create();
        }
    }
}
