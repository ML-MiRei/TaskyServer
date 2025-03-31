using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectService.Infrastructure.Database.Entities;

namespace ProjectService.Infrastructure.Database
{
    public class ProjectsDbContext(IOptions<DbConnectionOptions> options) : DbContext
    {

        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<BoardEntity> Boards { get; set; }
        public DbSet<MemberEntity> Members { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseNpgsql(options.Value.GetConnectionString());
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {


            mb.Entity<ProjectEntity>().HasKey(p => p.Id);
            mb.Entity<BoardEntity>().HasKey(p => new {p.BoardId, p.ProjectId} );
            mb.Entity<MemberEntity>().HasKey(m => new { m.UserId, m.ProjectId });
            mb.Entity<RoleEntity>().HasKey(c => c.Id);



            mb.Entity<MemberEntity>().HasOne(m => m.Project)
                                     .WithMany(p => p.Members)
                                     .HasForeignKey(p => p.ProjectId)
                                     .IsRequired()
                                     .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<MemberEntity>().HasOne(m => m.Role)
                                     .WithMany(p => p.Members)
                                     .HasForeignKey(p => p.RoleId)
                                     .IsRequired();

            mb.Entity<BoardEntity>().HasOne(c => c.Project)
                                     .WithMany(p => p.Sprints)
                                     .HasForeignKey(p => p.ProjectId)
                                     .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(mb);
        }
    }
}
