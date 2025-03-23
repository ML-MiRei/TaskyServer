using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BoardService.Infrastructure.Database.Entities;

namespace BoardService.Infrastructure.Database
{
    public class BoardDbContext(IOptions<DbConnectionOptions> options) : DbContext
    {

        public DbSet<BoardEntity> Boards { get; set; }
        public DbSet<BoardTypeEntity> Types { get; set; }
        public DbSet<SprintEntity> Sprints { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ExecutionStageEntity> Stages { get; set; }


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


            mb.Entity<BoardEntity>().HasKey(b => b.Id);
            mb.Entity<BoardTypeEntity>().HasKey(bt => bt.Id);
            mb.Entity<SprintEntity>().HasKey(s => s.Id);
            mb.Entity<TaskEntity>().HasKey(t => t.Id);
            mb.Entity<ExecutionStageEntity>().HasKey(es => es.Id);



            mb.Entity<BoardEntity>().HasOne(pt => pt.Type)
                                          .WithMany(p => p.Boards)
                                          .HasForeignKey(p => p.TypeId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<SprintEntity>().HasOne(pt => pt.Board)
                                          .WithMany(m => m.Sprints)
                                          .HasForeignKey(m => m.BoardId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.SetNull);

            mb.Entity<TaskEntity>().HasOne(pt => pt.Sprint)
                                          .WithMany(s => s.Tasks)
                                          .HasForeignKey(s => s.SprintId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.SetNull);
            mb.Entity<TaskEntity>().HasOne(pt => pt.Board)
                                          .WithMany(s => s.Tasks)
                                          .HasForeignKey(s => s.BoardId)
                                          .IsRequired(false)
                                          .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<TaskEntity>().HasOne(pt => pt.ExecutionStage)
                              .WithMany(s => s.Tasks)
                              .HasForeignKey(s => s.StageId)
                              .IsRequired(false)
                              .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<ExecutionStageEntity>().HasOne(c => c.Board)
                                      .WithMany(p => p.ExecutionStages)
                                      .HasForeignKey(p => p.BoardId)
                                      .OnDelete(DeleteBehavior.Cascade);
          

            base.OnModelCreating(mb);
        }
    }
}
