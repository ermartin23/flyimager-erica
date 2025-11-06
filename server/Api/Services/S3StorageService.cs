using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Api.Options;



namespace Api.Services;

public interface IObjectStorageService
{
    Task UploadAsync(string key, Stream content, CancellationToken ct = default);
    Task<Stream> GetAsync(string key, CancellationToken ct = default);
}

public class S3StorageService : IObjectStorageService
{
    private readonly IAmazonS3 _s3;
    private readonly S3Options _options;

    public S3StorageService(IOptions<S3Options> options)
    {
        _options = options.Value;

        _s3 = new AmazonS3Client(
            _options.AwsAccessKeyId,
            _options.AwsSecretAccessKey,
            new AmazonS3Config
            {
                ServiceURL = _options.AwsEndpointUrlS3,
                UseHttp = false
            });
    }

    public async Task UploadAsync(string key, Stream content, CancellationToken ct = default)
    {
        var put = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            InputStream = content,
            UseChunkEncoding = false
        };

        await _s3.PutObjectAsync(put, ct);
    }

    public async Task<Stream> GetAsync(string key, CancellationToken ct = default)
    {
        var res = await _s3.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key
        }, ct);

        return res.ResponseStream;
    }
}