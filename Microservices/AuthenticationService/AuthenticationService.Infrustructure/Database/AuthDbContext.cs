using AuthenticationService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Infrastructure.Database
{
    public class AuthDbContext(IOptions<DbConnectionOptions> options): DbContext
    {
        public DbSet<AuthData> AuthData { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(options.Value.GetConnectionString());
                optionsBuilder.EnableSensitiveDataLogging();
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthData>().HasKey(ad => ad.UserId);
            modelBuilder.Entity<AuthData>().HasIndex(ad => ad.Email).IsUnique();
            modelBuilder.Entity<AuthData>().Property(ad => ad.RegistrationDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<AuthData>().Property(ad => ad.UserId).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<AuthData>().Property(ad => ad.IsVerified).HasDefaultValueSql("False");
        }

    }
}
