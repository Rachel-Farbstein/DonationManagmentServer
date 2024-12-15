using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{
    [Table("Donations")]
    public class Donation
    {
        public int DonationId { get; set; }
        public int DonorId { get; set; }
        public int UserId { get; set; }
        public DateTime DonationDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }


        [ForeignKey("DonorId")]
        public Donor Donor { get; set; }
    }

}
