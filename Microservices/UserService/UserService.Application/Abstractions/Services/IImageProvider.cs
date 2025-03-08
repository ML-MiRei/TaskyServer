using UserService.Core.Common;

namespace UserService.Application.Abstractions.Services
{
    public interface IImageProvider
    {
        Task<Result<(byte[], string)>> GetImage(string key);
        Task<Result<string>> Upload(string key, byte[] image, string fileExtension);
    }
}