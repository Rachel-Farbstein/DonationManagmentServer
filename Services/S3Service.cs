using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DonationManagmentServer.Models.DTO;
using Microsoft.CodeAnalysis;

namespace DonationManagmentServer.Services
{

    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var bukName = configuration["AWS:BucketName"];
            if (bukName == null)
                throw new ArgumentNullException(nameof(bukName));
            _bucketName = bukName;
            _s3Client = new AmazonS3Client(
                configuration["AWS:AccessKey"],
                configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
            );
        }

        public async Task<FileS3Dto> UploadFileAsync(string congitoUserId, IFormFile file)
        {
            var transferUtility = new TransferUtility(_s3Client);

            // Upload the file stream to S3
            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();

            string uniqueKey = $"{congitoUserId}/{currentYear}/{currentMonth}/{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}";

            var Metadata = new Dictionary<string, string>
                {
                 { "UserId", congitoUserId }
                };
            using (var stream = file.OpenReadStream())
            {
                // Create the request
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = uniqueKey,
                    InputStream = stream,
                    ContentType = file.ContentType,
                };

                request.Metadata.Add("UserId", congitoUserId);

                var fileS3Dto = new FileS3Dto
                {
                    UniqueKey = uniqueKey,
                    FileUrl = $"https://{_bucketName}.s3.amazonaws.com/{uniqueKey}",
                    BucketName = _bucketName
                };
                // Upload to S3
                try
                {
                    var res = await _s3Client.PutObjectAsync(request);
                    return fileS3Dto;
                }
                catch (AmazonS3Exception ex) when (ex.StatusCode == (System.Net.HttpStatusCode)418)
                {
                    Console.WriteLine($"Ignored 418 error during S3 upload: {ex.Message}");
                    return fileS3Dto;

                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    throw;
                }

            }

        }
    }

}
