using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models.DTO
{
    public class DonationDto
    {
        public int DonationId { get; set; }
        public int DonorId { get; set; }
        public DateTime DonationDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public string? Notes { get; set; }
    }

}
