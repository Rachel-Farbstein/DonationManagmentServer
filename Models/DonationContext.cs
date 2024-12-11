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

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Donor> Donors { get; set; } = null!;
        public DbSet<DonationManagmentServer.Models.User> User { get; set; } = default!;
        public DbSet<DonationManagmentServer.Models.Donor> Donor { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הגדרת מפתחות ראשיים
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Donor>().HasKey(f => f.DonorId);

        }

    }
}
