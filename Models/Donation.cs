using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{
    [Table("Donations")]
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }
        public int DonorId { get; set; }
        public DateTime DonationDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public string? Notes { get; set; }


        [ForeignKey("DonorId")]
        public Donor Donor { get; set; }
        public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
    }

}
