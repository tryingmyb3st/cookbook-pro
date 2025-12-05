using Microsoft.AspNetCore.Http;

namespace CookbookFileStorage;

public interface IFileService
{
    Task<bool> BucketExistsAsync(string bucketName);
    Task CreateBucketIfNotExistsAsync(string bucketName);
    Task<string> UploadFileAsync(IFormFile file, string fileName);
    Task<Stream> DownloadFileAsync(string fileName);
    Task<List<string>> ListFilesAsync(string prefix);
    Task DeleteFileAsync(string fileName);
    Task<string> GetFileUrlAsync(string fileName, int expirySeconds = 3600);
    string GetPublicUrl(string fileName);
}