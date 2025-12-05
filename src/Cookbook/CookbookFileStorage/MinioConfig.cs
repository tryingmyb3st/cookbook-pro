namespace CookbookFileStorage;

public class MinioConfig
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    public string Region { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; }
    public string PublicUrl { get; set; } = string.Empty;
}