using Microsoft.EntityFrameworkCore;
using DonationManagmentServer.Models;


namespace DonationManagmentServer.Models
{
    public class DonationContext : DbContext
    {
        public DonationContext(DbContextOptions<DonationContext> options)
       : base(options)
        {
        }

        public DbSet<Donation> Donataions { get; set; } = null!;
        public DbSet<DonationManagmentServer.Models.Donor> Donor { get; set; } = default!;

    }
}
