using Amazon.S3;
using Amazon.S3.Transfer;

namespace DonationManagmentServer.Services
{

    public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IConfiguration configuration)
    {
        _bucketName = configuration["AWS:BucketName"];
        _s3Client = new AmazonS3Client(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
        );
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var transferUtility = new TransferUtility(_s3Client);
        using (var stream = file.OpenReadStream())
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = file.FileName,
                BucketName = _bucketName,
                ContentType = file.ContentType
            };
            await transferUtility.UploadAsync(uploadRequest);
        }
        return $"https://{_bucketName}.s3.amazonaws.com/{file.FileName}";
    }
}

}
