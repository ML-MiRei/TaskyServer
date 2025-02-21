namespace ProjectService.Application.DTOs
{
    public class SprintDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
