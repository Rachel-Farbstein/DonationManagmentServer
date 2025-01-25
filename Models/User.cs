using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagmentServer.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string CognitoUserId { get; set; }
        public string CognitoUserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public bool IsActive { get; set; }


        public ICollection<Donor> Donors { get; set; } = new List<Donor>();
        public ICollection<FileDetails> Files { get; set; } = new List<FileDetails>();
    }

}
