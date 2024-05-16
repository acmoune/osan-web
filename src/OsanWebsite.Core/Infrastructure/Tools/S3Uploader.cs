using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace OsanWebsite.Core.Infrastructure.Tools;

public static class S3Uploader
{
    public static async Task Upload (Stream stream, string key)
    {
        var bucketName = Environment.GetEnvironmentVariable("OSAN_S3_BucketName");
        var accessKey = Environment.GetEnvironmentVariable("OSAN_S3_AccessKey");
        var secretKey = Environment.GetEnvironmentVariable("OSAN_S3_SecretKey");

        BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
        AmazonS3Client client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);

        TransferUtility transferUtility = new TransferUtility(client);

        var req = new TransferUtilityUploadRequest
        {
            BucketName = bucketName,
            Key = key,
            ContentType = "images/png",
            InputStream = stream,
            CannedACL = S3CannedACL.PublicRead
        };

        await transferUtility.UploadAsync(req);
    }
}
