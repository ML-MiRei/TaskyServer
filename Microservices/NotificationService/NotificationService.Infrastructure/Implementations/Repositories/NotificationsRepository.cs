﻿using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Core.Enums;
using NotificationService.Core.Models;
using NotificationService.Infrastructure.Database;
using NotificationService.Infrastructure.Database.Entities;

namespace NotificationService.Infrastructure.Implementations.Repositories
{
    public class NotificationsRepository(NotificationDbContext context) : INotificationsRepository
    {
        public async Task<int> CreateAsync(string userid, NotificationModel notificationModel)
        {
            var notification = new NotificationEntity
            {
                CreatedDate = notificationModel.CreatedDate,
                ObjectId = notificationModel.Object.Id,
                ObjectTypeId = notificationModel.Object.ObjectType.Id,
                Text = notificationModel.Text,
                Title = notificationModel.Title,
                UserId = userid
            };

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<List<NotificationModel>> CreateAsync(string[] userid, NotificationModel notificationModel)
        {
            var res = new List<NotificationModel>();

            foreach (var user in userid)
            {
                var notification = new NotificationEntity
                {
                    CreatedDate = notificationModel.CreatedDate,
                    ObjectId = notificationModel.Object.Id,
                    ObjectTypeId = notificationModel.Object.ObjectType.Id,
                    Text = notificationModel.Text,
                    Title = notificationModel.Title,
                    UserId = user
                };

                await context.Notifications.AddAsync(notification); // warning

                notificationModel.Id = notification.Id;
                res.Add(notificationModel);
            }

            await context.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var notification = await context.Notifications.FindAsync(id);

            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<List<NotificationModel>> GetAllByUserId(string userId)
        {
            var notifications = context.Notifications.AsNoTracking()
                .Where(n => n.UserId == userId)
                .Select(n => new NotificationModel(n.Title, n.Text, n.CreatedDate, new NotificationObjectValOb(n.ObjectId, new ObjectTypeValOb((ObjectTypes)n.ObjectType.Id)), n.Id))
                .ToList();

            return notifications;
        }
    }
}
