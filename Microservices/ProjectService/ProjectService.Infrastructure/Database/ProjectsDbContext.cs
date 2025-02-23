using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectService.Infrastructure.Database.Entities;

namespace ProjectService.Infrastructure.Database
{
    public class ProjectsDbContext(IOptions<DbConnectionOptions> options) : DbContext
    {

        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<ProjectTaskEntity> ProjectTasks { get; set; }
        public DbSet<SprintEntity> Sprints { get; set; }
        public DbSet<MemberEntity> Members { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<StatusTaskEntity> Statuses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseNpgsql(options.Value.GetConnectionString());
            }

            Database.EnsureCreated();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {


            mb.Entity<ProjectEntity>().HasKey(p => p.Id);
            mb.Entity<ProjectTaskEntity>().HasKey(p =>  p.Id);
            mb.Entity<SprintEntity>().HasKey(p => p.Id );
            mb.Entity<MemberEntity>().HasKey(m => m.UserId);
            mb.Entity<CommentEntity>().HasKey(c => c.Id);
            mb.Entity<StatusTaskEntity>().HasKey(c => c.Id );
            mb.Entity<FileEntity>().HasKey(c => c.Id);
            mb.Entity<RoleEntity>().HasKey(c => c.Id);


            mb.Entity<ProjectTaskEntity>().HasOne(pt => pt.Project)
                                          .WithMany(p => p.ProjectTasks)
                                          .HasForeignKey(p => p.ProjectId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<ProjectTaskEntity>().HasOne(pt => pt.Member)
                                          .WithMany(m => m.ProjectTasks)
                                          .HasForeignKey(m => m.ExecutorId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.SetNull);
            mb.Entity<ProjectTaskEntity>().HasOne(pt => pt.Sprint)
                                          .WithMany(s => s.ProjectTasks)
                                          .HasForeignKey(s => s.SprintId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.SetNull);
            mb.Entity<ProjectTaskEntity>().HasOne(pt => pt.Status)
                                          .WithMany(s => s.ProjectTasks)
                                          .HasForeignKey(s => s.StatusId)
                                          .IsRequired()
                                          .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<CommentEntity>().HasOne(c => c.ProjectTask)
                                      .WithMany(p => p.Comments)
                                      .HasForeignKey(p => new { p.ProjectId, p.ProjectTaskId, p.Id })
                                      .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<CommentEntity>().HasOne(c => c.Member)
                                      .WithMany(p => p.Comments)
                                      .HasForeignKey(p => p.AuthorId)
                                      .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<MemberEntity>().HasOne(m => m.Project)
                                     .WithMany(p => p.Members)
                                     .HasForeignKey(p => p.ProjectId)
                                     .IsRequired()
                                     .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<MemberEntity>().HasOne(m => m.Role)
                                     .WithMany(p => p.Members)
                                     .HasForeignKey(p => p.RoleId)
                                     .IsRequired();

            mb.Entity<SprintEntity>().HasOne(c => c.Project)
                                     .WithMany(p => p.Sprints)
                                     .HasForeignKey(p => p.ProjectId)
                                     .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<FileEntity>().HasOne(pt => pt.ProjectTask)
                                   .WithMany(s => s.Files)
                                   .HasForeignKey(s => s.ProjectTaskId)
                                   .IsRequired(false)
                                   .OnDelete(DeleteBehavior.SetNull);
            mb.Entity<FileEntity>().HasOne(pt => pt.Project)
                                   .WithMany(s => s.Files)
                                   .HasForeignKey(s => s.ProjectId)
                                   .IsRequired()
                                   .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(mb);
        }
    }
}
