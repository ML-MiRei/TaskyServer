namespace ProjectService.Application.DTOs
{
    public class ProjectTaskDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? Executor { get; set; }
        public Guid Project { get; }
        public int? Sprint { get; set; }
        public int Status { get; set; }
    }
}
