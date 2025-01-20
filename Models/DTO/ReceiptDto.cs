namespace DonationManagmentServer.Models.DTO
{
    public class ReceiptDto
    {
        public int ReceiptId { get; set; }
        public int DonationId { get; set; }
        public DateTime ReceiptProductionDate { get; set; }
        public IFormFile? File { get; set; }
    }
}
