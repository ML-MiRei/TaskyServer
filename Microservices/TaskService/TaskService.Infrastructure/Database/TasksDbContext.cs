using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure.Database
{
    public class TasksDbContext(IOptions<DbConnectionOptions> options) : DbContext
    {

        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<StatusEntity> Statuses { get; set; }
        public DbSet<ExecutionEntity> Executions { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }


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

            mb.Entity<CommentEntity>().HasKey(c => c.Id);
            mb.Entity<StatusEntity>().HasKey(s => s.Id);
            mb.Entity<ExecutionEntity>().HasKey(e => e.Id);
            mb.Entity<TaskEntity>().HasKey(t => t.Id);

            mb.Entity<CommentEntity>().HasOne(pt => pt.Task)
                                          .WithMany(p => p.Comments)
                                          .HasForeignKey(p => p.TaskId)
                                          .IsRequired(true)
                                          .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<ExecutionEntity>().HasOne(pt => pt.Task)
                                          .WithMany(m => m.Executions)
                                          .HasForeignKey(m => m.TaskId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.SetNull);
            mb.Entity<ExecutionEntity>().HasOne(pt => pt.Status)
                                          .WithMany(m => m.Executions)
                                          .HasForeignKey(m => m.StatusId)
                                          .IsRequired(true)
                                          .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(mb);
        }
    }
}
