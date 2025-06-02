namespace Gateaway.Core.ReplyModels
{
    public class ProjectModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string[] BoardIds { get; set; }
        public MemberModel[] Members { get; set; }

    }
}
