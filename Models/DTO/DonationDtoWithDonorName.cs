using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models.DTO
{
    public class DonationDtoWithDonorName
    {
        public DonationDto? Donation { get; set; }
        public string? DonorName { get; set; }
        public FileDetailsDto? fileDetailsDto { get; set; }
    }

}
