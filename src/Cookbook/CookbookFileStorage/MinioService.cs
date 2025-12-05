using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;

namespace CookbookFileStorage;

public class MinioService(IMinioClient minioClient, MinioConfig config) : IFileService
{
    private readonly IMinioClient _minioClient = minioClient;
    private readonly MinioConfig _config = config;

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        try
        {
            var bucket = bucketName ?? _config.BucketName;
            var args = new BucketExistsArgs().WithBucket(bucket);
            return await _minioClient.BucketExistsAsync(args);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateBucketIfNotExistsAsync(string bucketName)
    {
        try
        {
            var bucket = bucketName ?? _config.BucketName;

            var exists = await BucketExistsAsync(bucket);
            if (!exists)
            {
                var args = new MakeBucketArgs().WithBucket(bucket);
                await _minioClient.MakeBucketAsync(args);

                await SetBucketPolicyAsync(bucket);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task SetBucketPolicyAsync(string bucketName)
    {
        var policy = $@"{{
                    ""Version"": ""2012-10-17"",
                    ""Statement"": [
                        {{
                            ""Effect"": ""Allow"",
                            ""Principal"": ""*"",
                            ""Action"": ""s3:GetObject"",
                            ""Resource"": ""arn:aws:s3:::{bucketName}/*""
                        }}
                    ]
                }}";

        var args = new SetPolicyArgs()
            .WithBucket(bucketName)
            .WithPolicy(policy);

        await _minioClient.SetPolicyAsync(args);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string fileName)
    {
        try
        {
            await CreateBucketIfNotExistsAsync(_config.BucketName);

            var bucketName = _config.BucketName;
            var objectName = fileName ?? $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            using var fileStream = file.OpenReadStream();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await _minioClient.PutObjectAsync(putObjectArgs);
            return objectName;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await _minioClient.GetObjectAsync(args);

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<string>> ListFilesAsync(string prefix)
    {
        var fileNames = new List<string>();

        try
        {
            var args = new ListObjectsArgs()
                .WithBucket(_config.BucketName);

            if (!string.IsNullOrEmpty(prefix))
            {
                args = args.WithPrefix(prefix);
            }

            var observable = _minioClient.ListObjectsAsync(args);

            var subscription = observable.Subscribe(
                item => fileNames.Add(item.Key),
                () => { });

            await Task.Delay(100);
        }
        catch (Exception)
        {
            throw;
        }

        return fileNames;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(args);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> GetFileUrlAsync(string fileName, int expirySeconds = 3600)
    {
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName)
                .WithExpiry(expirySeconds);

            return await _minioClient.PresignedGetObjectAsync(args);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string GetPublicUrl(string fileName)
    {
        var baseUrl = _config.PublicUrl.TrimEnd('/');
        var bucketName = _config.BucketName;
        return $"{baseUrl}/{bucketName}/{fileName}";
    }
}
