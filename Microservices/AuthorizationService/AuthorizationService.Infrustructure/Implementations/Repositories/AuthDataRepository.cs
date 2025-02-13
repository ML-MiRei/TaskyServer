using AuthenticationService.Applicaion.Abstractions.Repositories;
using AuthenticationService.Core.Models;
using AuthenticationService.Infrastructure.Database;
using AuthenticationService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Implementations.Repositories
{
    public class AuthDataRepository(AuthDbContext context) : IAuthDataRepository
    {
        public async Task<Guid> Create(AuthDataModel authData)
        {
            var newData = new AuthData(authData.Email, authData.PasswordHash);
            await context.AuthData.AddAsync(newData);
            await context.SaveChangesAsync();
            
            return newData.UserId;
        }

        public async Task<AuthDataModel?> GetByEmail(string email)
        {
            var data = await context.AuthData.FirstOrDefaultAsync(ad => ad.Email == email);
            return data == null ? null : AuthDataModel.Create(data.Email, data.PasswordHash, data.UserId).Value;
        }

        public async Task<Guid?> UpdateEmailAsync(Guid userId, string email)
        {
            var data = await context.AuthData.FirstOrDefaultAsync(ad => ad.UserId == userId);

            if(data == null) 
                return null;

            data.Email = email;
            await context.SaveChangesAsync();

            return userId;
        }

        public async Task<Guid?> UpdatePasswordAsync(Guid userId, string passwordHash)
        {
            var data = await context.AuthData.FirstOrDefaultAsync(ad => ad.UserId == userId);

            if (data == null)
                return null;

            data.PasswordHash = passwordHash;
            await context.SaveChangesAsync();

            return userId;
        }
    }
}
