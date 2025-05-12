namespace Gateaway.Core.ReplyModels
{
    public class ProjectModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<BoardModel> Boards { get; set; }
        public List<UserModel> Members { get; set; }
        public List<TaskModel> Tasks { get; set; }

    }
}
