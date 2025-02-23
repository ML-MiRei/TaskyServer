namespace ProjectService.Infrastructure.Database.Entities
{
    public class StatusTaskEntity
    {
        public int Id  { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }

        public List<ProjectTaskEntity> ProjectTasks { get; set; }
    }
}
