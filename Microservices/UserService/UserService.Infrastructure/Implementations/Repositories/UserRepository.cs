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


        public async Task<UserModel> CreateAsync(string id, string email)
        {
            var newUser = new User
            {
                Email = email,
                Id = id.ToString(),
                Name = FormatEmailToUsername(email)
            };

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            var userModel = UserModel.Create(id, newUser.Name, email).Value;

            return userModel;
        }


        public async Task<string> UpdateAsync(UserModel userModel)
        {
            var updatingUser = context.Users.First(u => u.Id == userModel.Id.ToString());

            updatingUser.PictureKey = userModel.ProfilePicture?.Name;
            updatingUser.Phone = userModel.Phone;
            updatingUser.Name = userModel.Name;
            updatingUser.Gender = (int)userModel.Gender;

            context.Users.Update(updatingUser);
            await context.SaveChangesAsync();

            return userModel.Id;
        }

        public async Task<UserModel> GetByIdAsync(string userId)
        {
            var user = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == userId);

            var userModel = UserModel.Create(userId, user.Name, user.Email, new Picture { Name = user.PictureKey ?? "" }, user.Phone, (GenderCode)user.Gender).Value;

            return userModel;
        }


        public async Task<List<UserModel>> GetByIdAsync(string[] userIds)
        {
            var ids = userIds.Select(u => u.ToString());

            var users = context.Users
                .AsNoTracking()
                .Where(u => ids.Contains(u.Id))
                .Select(u => UserModel.Create(u.Id, u.Name, u.Email, new Picture { Name = u.PictureKey }, u.Phone, (GenderCode)u.Gender).Value)
                .ToList();

            return users;
        }


        public async Task<List<UserModel>> FindByNameAsync(string userName)
        {
            userName = userName.ToLower().Trim();

            var users = context.Users
                .AsNoTracking()
                .Where(u => u.Name.ToLower().Contains(userName))
                .Select(u => UserModel.Create(u.Id, u.Name, u.Email, new Picture { Name = u.PictureKey }, u.Phone, (GenderCode)u.Gender).Value)
                .ToList();

            return users;
        }

    }
}
