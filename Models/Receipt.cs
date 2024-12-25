using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{

    [Table("Receipts")]
    public class Receipt
    {
        public int ReceiptID { get; set; }
        public int DonationID { get; set; }
        public int FileID { get; set; }
        public DateTime ReceiptProductionDate { get; set; }

        [ForeignKey("DonationID")]
        public Donation Donation { get; set; }

        [ForeignKey("FileID")]
        public FileS3 FileS3 { get; set; }

    }
}
