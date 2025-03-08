namespace FilesProvider.Settings
{
    public sealed class S3Settings
    {
        public string Region { get; set; }
        public string BucketName { get; set; }
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
