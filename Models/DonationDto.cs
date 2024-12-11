using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{
    [Table("Donations")]
    public class DonationDto
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public int DonorId { get; set; }
        public DateTime DonationDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
    }

}
