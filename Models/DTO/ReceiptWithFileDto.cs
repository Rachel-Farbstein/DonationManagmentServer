using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models.DTO
{
    public class ReceiptWithFileDto
    {
        public required ReceiptDto ReceiptDto { get; set; }
        public required FileDetailsDto FileDetailsDto { get; set; }
    }

}
