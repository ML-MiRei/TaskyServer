using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.Database.Entities;

namespace UserService.Infrastructure.Database
{
    public class UserDbContext(IOptions<DbConnectionOptions> options): DbContext
    {
        public DbSet<User> Users { get; set; }


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
            modelBuilder.Entity<User>().HasKey(ad => ad.Id);
            modelBuilder.Entity<User>().HasIndex(ad => ad.Name);
        }
    }
}
