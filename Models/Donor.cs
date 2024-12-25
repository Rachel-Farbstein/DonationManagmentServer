using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{
    [Table("Donors")]
    public class Donor
    {
        [Key]
        public int DonorId { get; set; }
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }


        [ForeignKey("UserId")]
        public User? User { get; set; }
        public ICollection<Donation>? Donations { get; set; } = new List<Donation>();

    }
}
