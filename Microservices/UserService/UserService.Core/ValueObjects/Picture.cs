namespace UserService.Core.ValueObjects
{
    public class Picture
    {
        public string Name { get; set; } = String.Empty;
        public string Extension { get; set; } = String.Empty ;
        public byte[] Bytes { get; set; }

    }
}
