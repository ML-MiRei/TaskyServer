namespace Gateaway.Core.ReplyModels
{
    public class StageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Queue { get; set; }
        public int? MaxTasksCount { get; set; }
    }
}
