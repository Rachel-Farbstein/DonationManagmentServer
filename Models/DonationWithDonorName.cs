using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models.DTO
{
    public class DonationWithDonorName
    {
        public Donation? Donation { get; set; }
        public string? DonorName { get; set; }
    }

}
