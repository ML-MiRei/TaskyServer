using Grpc.Core;
using NotificationService.Application.Abstractions.Repositories;

namespace NotificationService.API.Services
{
    public class NotificationsService(ILogger<NotificationsService> logger, INotificationsRepository notificationsRepository) : Notifications.NotificationsBase
    {
        public override async Task<DeleteReply> Delete(DeleteRequest request, ServerCallContext context)
        {
            try
            {
                var notifId = await notificationsRepository.DeleteAsync(request.NotificationId);
                return new DeleteReply { NotificationId = notifId };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка удаления. Повторите попытку позже"));
            }
        }

        public override async Task<GetAllByUserIdReply> GetAllByUserId(GetAllByUserIdRequest request, ServerCallContext context)
        {
            try
            {
                var notifications = await notificationsRepository.GetAllByUserId(request.UserId);
                var reply = new GetAllByUserIdReply();

                reply.Notifications.AddRange(notifications.Select(n => new NotificationInfo
                {
                    NotificationId = n.Id.Value,
                    ObjectId = n.Object.Id,
                    Text = n.Text,
                    Title = n.Title,
                    ObjectType = n.Object.ObjectType.Id
                }));

                return reply;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка получения данных. Повторите попытку позже"));
            }
        }
    }
}
