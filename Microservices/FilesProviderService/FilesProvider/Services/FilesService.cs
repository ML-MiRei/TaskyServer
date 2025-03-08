using Amazon.S3;
using Amazon.S3.Model;
using FilesProvider.Settings;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace FilesProvider.Services
{
    public class FilesService(ILogger<FilesService> logger, IAmazonS3 s3Client, IOptions<S3Settings> s3Settings) : FilesProvider.FilesProviderBase
    {
        public const string CONTENT_TYPE = "application/octet-stream";
        public const string FOLDER = "images";

        public override async Task<GetFileReply> GetFile(GetFileRequest request, ServerCallContext context)
        {
            logger.LogDebug($"Start search file key={request.Key}");
            //maybe can change on url
            var getRequest = new GetObjectRequest
            {
                BucketName = s3Settings.Value.BucketName,
                Key = $"{FOLDER}/{request.Key}"
            };

            var response = await s3Client.GetObjectAsync(getRequest);

            logger.LogDebug($"File key={request.Key} is founded");

            return new GetFileReply
            {
                ChunkData = ByteString.FromStream(response.ResponseStream),
                FileType = response.Metadata["file-type"]
            };

        }

        public override async Task<DeleteFileReply> DeleteFile(DeleteFileRequest request, ServerCallContext context)
        {

            logger.LogDebug($"Start deleting file key={request.Key}");

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = s3Settings.Value.BucketName,
                Key = $"{FOLDER}/{request.Key}"
            };

            await s3Client.DeleteObjectAsync(deleteRequest);

            logger.LogDebug($"File key={request.Key} is deleted");

            return new DeleteFileReply
            {
                Key = request.Key,
            };
        }

        public override async Task<UploadFileReply> UploadFile(UploadFileRequest request, ServerCallContext context)
        {
            logger.LogDebug($"Start upload file key={request.Key}");

            using (FileStream fstream = new FileStream($"temp.{request.FileType}", FileMode.OpenOrCreate))
            {
                await fstream.WriteAsync(request.ChunkData.ToByteArray(), 0, request.ChunkData.Length);
                var putRequest = new PutObjectRequest
                {
                    BucketName = s3Settings.Value.BucketName,
                    Key = $"{FOLDER}/{request.Key}",
                    InputStream = fstream,
                    ContentType = CONTENT_TYPE,
                    Metadata =
                    {
                        ["file-name"] = request.Key,
                        ["file-type"] = request.FileType
                    }
                };

                await s3Client.PutObjectAsync(putRequest);

                logger.LogDebug($"File key={request.Key} is uploaded");

                return new UploadFileReply { Key = request.Key };
            }

        }
    }
}
