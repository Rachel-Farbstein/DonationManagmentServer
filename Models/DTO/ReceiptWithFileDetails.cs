using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models.DTO
{
    public class ReceiptWithFileDetails
    {
        public ReceiptDto? Receipt { get; set; }
        public FileS3Dto? FileS3Dto { get; set; }
    }

}
