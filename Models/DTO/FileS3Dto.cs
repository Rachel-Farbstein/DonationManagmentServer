namespace DonationManagmentServer.Models.DTO
{
    public class FileS3Dto
    {
        public required string S3FileKey { get; set; }
        public required string S3FileUrl { get; set; }
        public required string S3BucketName { get; set; }

    }
}
