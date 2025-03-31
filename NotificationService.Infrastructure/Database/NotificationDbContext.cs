using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotificationService.Infrastructure.Database.Entities;

namespace NotificationService.Infrastructure.Database
{
    public class NotificationDbContext(IOptions<DbConnectionOptions> options) : DbContext
    {
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<ObjectTypeEntity> ObjectTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseNpgsql(options.Value.GetConnectionString());
            }

            //Database.EnsureCreated();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<NotificationEntity>().HasKey(b => b.Id);
            mb.Entity<ObjectTypeEntity>().HasKey(bt => bt.Id);

            mb.Entity<NotificationEntity>().HasOne(pt => pt.ObjectType)
                                          .WithMany(p => p.Notifications)
                                          .HasForeignKey(p => p.ObjectTypeId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(mb);
        }
    }
}
