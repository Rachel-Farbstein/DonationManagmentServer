using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{

    [Table("Files")]
    public class FileDto
    {

        public int DonorId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string S3FileKey { get; set; }
        public string S3FileUrl { get; set; }
        public string S3BucketName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}
