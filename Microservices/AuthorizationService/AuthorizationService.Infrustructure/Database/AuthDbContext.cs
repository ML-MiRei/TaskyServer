using AuthenticationService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace AuthenticationService.Infrastructure.Database
{
    public class AuthDbContext : DbContext
    {

        public DbSet<AuthData> AuthData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
                optionsBuilder.EnableSensitiveDataLogging();
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthData>().HasKey(ad => ad.UserId);
            modelBuilder.Entity<AuthData>().HasIndex(ad => ad.Email).IsUnique();
        }

    }
}
