namespace DonationManagmentServer.Models.DTO
{
    public class FileDetailsDto
    {
        public int FileId { get; set; }
        public required string FileName { get; set; }
        public required string S3FileKey { get; set; }
        public required string S3FileUrl { get; set; }
        public required string S3BucketName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
