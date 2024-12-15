using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace DonationManagmentServer.Services
{

    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string? _bucketName;

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

            // Upload the file stream to S3
            string userId = "12345"; // Example user ID
            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();

            string uniqueKey = $"{userId}/{currentYear}/{currentMonth}/{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}";


            using (var stream = file.OpenReadStream())
            {
                // Create the request
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = uniqueKey,
                    InputStream = stream,
                    ContentType = file.ContentType
                };

                // Upload to S3
                try
                {
                    var res = await _s3Client.PutObjectAsync(request);
                }
                catch (Exception ex)
                {
                    return uniqueKey;

                }

                return $"https://{_bucketName}.s3.amazonaws.com/{uniqueKey}";
            }
            //return $"https://{_bucketName}.s3.amazonaws.com/{file.FileName}";
        }
    }

}
