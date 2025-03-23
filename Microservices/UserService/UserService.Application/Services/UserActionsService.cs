using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Abstractions.Services;
using UserService.Core.Common;
using UserService.Core.Models;

namespace UserService.Application.Services
{
    public class UserActionsService(IUserRepository repository, IImageProvider imageProvider, ILogger<UserActionsService> logger)
    {
        public async Task<Result<UserModel>> CreateUserAsync(string id, string email)
        {
            var resultFactory = new ResultFactory<UserModel>();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(email))
            {
                resultFactory.AddError("Введены не все аргументы");
                return resultFactory.Create();
            }

            try
            {
                var user = await repository.CreateAsync(id, email);
                resultFactory.SetResult(user);
            }
            catch (Exception ex)
            {
                resultFactory.AddError(ex.Message);
                logger.LogError(ex.ToString());
            }

            return resultFactory.Create();
        }

        public async Task<Result<string>> UpdateUserAsync(UserModel userModel)
        {
            var resultFactory = new ResultFactory<string>();

            try
            {
                var imageKey = Regex.Replace(userModel.Id.ToString(), @"[^\w\.@-]", "");

                var response = await imageProvider.Upload(imageKey, userModel.ProfilePicture.Bytes, userModel.ProfilePicture.Extension);

                if (response.IsError)
                {
                    userModel.ClearProfilePicture();
                    resultFactory.AddError("Загрузка изображения не удалась");
                }

                var userId = await repository.UpdateAsync(userModel);
                resultFactory.SetResult(userId);
            }
            catch (Exception ex)
            {
                resultFactory.AddError(ex.Message);
                logger.LogError(ex.ToString());
            }

            return resultFactory.Create();
        }

        public async Task<Result<UserModel>> GetUserAsync(string id)
        {
            var resultFactory = new ResultFactory<UserModel>();

            try
            {
                var user = await repository.GetByIdAsync(id);
                if (user == null)
                    resultFactory.AddError("Пользователь не найден");

                resultFactory.SetResult(user);
            }
            catch (Exception ex)
            {
                resultFactory.AddError(ex.Message);
                logger.LogError(ex.ToString());
            }

            return resultFactory.Create();
        }


        public async Task<Result<List<UserModel>>> GetUsersAsync(string[] ids)
        {
            var resultFactory = new ResultFactory<List<UserModel>>();

            try
            {
                var user = await repository.GetByIdAsync(ids);
                if (user == null)
                    resultFactory.AddError("Пользователи не найдены");

                resultFactory.SetResult(user);
            }
            catch (Exception ex)
            {
                resultFactory.AddError(ex.Message);
                logger.LogError(ex.ToString());
            }

            return resultFactory.Create();
        }


        public async Task<Result<List<UserModel>>> FindUsersByNameAsync(string name)
        {
            var resultFactory = new ResultFactory<List<UserModel>>();

            try
            {
                var user = await repository.FindByNameAsync(name);
                if (user == null)
                    resultFactory.AddError("Пользователи не найдены");

                resultFactory.SetResult(user);
            }
            catch (Exception ex)
            {
                resultFactory.AddError(ex.Message);
                logger.LogError(ex.ToString());
            }

            return resultFactory.Create();
        }

    }
}
