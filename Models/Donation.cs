namespace DonationManagmentServer.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public int donorId { get; set; }
        public DateOnly donateDate { get; set; }
        public decimal sum { get; set; }
        public PayType payType { get; set; }
    }

}
