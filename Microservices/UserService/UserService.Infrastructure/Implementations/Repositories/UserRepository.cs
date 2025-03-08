using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using UserService.Application.Abstractions.Repositories;
using UserService.Core.Enums;
using UserService.Core.Models;
using UserService.Core.ValueObjects;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Database.Entities;

namespace UserService.Infrastructure.Implementations.Repositories
{
    public class UserRepository(UserDbContext context) : IUserRepository
    {
        public static string FormatEmailToUsername(string email)
        {
            string pattern = @"^([^@]+)@.*$";

            Match match = Regex.Match(email, pattern);
            return match.Groups[1].Value;
        }


        public async Task<UserModel> CreateAsync(Guid id, string email)
        {
            var newUser = new User
            {
                Email = email,
                Id = id,
                Name = FormatEmailToUsername(email)
            };

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            var userModel = UserModel.Create(id, newUser.Name, email).Value;

            return userModel;
        }


        public async Task<Guid> UpdateAsync(UserModel userModel)
        {
            var updatingUser = context.Users.AsNoTracking().First(u => u.Id == userModel.Id);

            updatingUser.PicturePath = userModel.ProfilePicture?.Name;
            updatingUser.Phone = userModel.Phone;
            updatingUser.Name = userModel.Name;
            updatingUser.Gender = (int)userModel.Gender;

            context.Users.Update(updatingUser);
            await context.SaveChangesAsync();

            return userModel.Id;
        }

        public async Task<UserModel> GetByIdAsync(Guid userId)
        {
            var user = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == userId);

            var userModel = UserModel.Create(userId, user.Name, user.Email, new Picture { Name = user.PicturePath }, user.Phone, (GenderCode)user.Gender).Value;

            return userModel;
        }


        public async Task<List<UserModel>> GetByIdAsync(Guid[] userIds)
        {
            var users = context.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => UserModel.Create(u.Id, u.Name, u.Email, new Picture { Name = u.PicturePath }, u.Phone, (GenderCode)u.Gender).Value)
                .ToList();

            return users;
        }


        public async Task<List<UserModel>> FindByNameAsync(string userName)
        {
            userName = userName.ToLower().Trim();

            var users = context.Users
                .AsNoTracking()
                .Where(u => u.Name.ToLower().Contains(userName))
                .Select(u => UserModel.Create(u.Id, u.Name, u.Email, new Picture { Name = u.PicturePath }, u.Phone, (GenderCode)u.Gender).Value)
                .ToList();

            return users;
        }

    }
}
