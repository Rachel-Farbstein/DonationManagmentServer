using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{
    public class ReceiptWithFile
    {
        public required Receipt Receipt { get; set; }
        public required FileDetails FileDetails { get; set; }
    }

}
