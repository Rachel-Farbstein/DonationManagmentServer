using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Models;

public class DonorContext : DbContext
{
    public DonorContext(DbContextOptions<DonorContext> options)
        : base(options)
    {
    }

    public DbSet<Donor> Donors { get; set; } = null!;
}
