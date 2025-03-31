using NotificationService.Application.Dtos;

namespace NotificationService.Application.Abstractions.Services
{
    public interface ITasksService
    {
        public Task<List<CountTasksInfoDto>> GetCountTasksInfo();
        public Task<List<CountTasksInfoDto>> GetExecutionsInfo();
    }
}
