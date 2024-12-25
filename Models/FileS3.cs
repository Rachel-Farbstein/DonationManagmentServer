using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{

    [Table("Files")]
    public class FileS3
    {
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; } = "";
        public string S3FileKey { get; set; }
        public string S3FileUrl { get; set; }
        public string S3BucketName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    }
}
