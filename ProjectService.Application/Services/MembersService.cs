using Microsoft.Extensions.Logging;
using ProjectService.Application.Abstractions.Repositories;
using ProjectService.Core.Common;
using ProjectService.Core.Models;

namespace ProjectService.Application.Services
{
    public class MembersService(IMembersRepository membersRepository, ILogger<MembersService> logger)
    {
        public async Task<Result<(Guid, Guid)>> AddAsync(Guid projectId, Guid userID)
        {
            var res = new ResultFactory<(Guid, Guid)>();

            try
            {
                var user = await membersRepository.AddAsync(projectId, userID);
                res.SetResult(user);

                // notificate / +- kafka

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка добаввления пользователя. Повторите попытку позже");
            }

            return res.Create();
        }


        public async Task<Result<(Guid, Guid)>> DeleteAsync(Guid projectId, Guid userID)
        {
            var res = new ResultFactory<(Guid, Guid)>();

            try
            {
                var addedUser = await membersRepository.DeleteAsync(projectId, userID);
                res.SetResult(addedUser);

                // notificate / +- kafka

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка удаления пользователя. Повторите попытку позже");
            }

            return res.Create();
        }


        public async Task<Result<List<MemberModel>>> GetAllAsync(Guid projectId)
        {
            var res = new ResultFactory<List<MemberModel>>();

            try
            {
                var members = await membersRepository.GetAllAsync(projectId);

                // request in user service

                res.SetResult(members);

                // cash
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка получения данных. Повторите попытку позже");
            }

            return res.Create();
        }

        public async Task<Result<(Guid, Guid)>> UpdateRoleAsync(Guid projectId, Guid userID, int roleId)
        {
            var res = new ResultFactory<(Guid, Guid)>();

            try
            {
                var user = await membersRepository.UpdateRoleAsync(projectId, userID, roleId);
                res.SetResult(user);

                // notificate / +- kafka

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                res.AddError("Ошибка обновления данных. Повторите попытку позже");
            }

            return res.Create();
        }
    }
}
