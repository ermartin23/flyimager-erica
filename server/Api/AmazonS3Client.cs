using Amazon.S3;
using Microsoft.Extensions.Options;

namespace flyimager.Services;


public class S3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IOptions<S3Options> options)
    {
       
        var cfg = options.Value;

       
        _s3Client = new AmazonS3Client(
            cfg.AwsAccessKeyId,
            cfg.AwsSecretAccessKey,
            new AmazonS3Config
            {
                ServiceURL = cfg.AwsEndpointUrlS3,
                UseHttp = false
            }
        );
    }

    public IAmazonS3 Client => _s3Client;
}