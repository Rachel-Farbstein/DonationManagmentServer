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

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Donor> Donor { get; set; } = default!;
        public DbSet<Donation> Donation { get; set; } = default!;
        public DbSet<FileS3> File { get; set; } = default!;
        public DbSet<Receipt> Receipt { get; set; } = default!;

        //public DbSet<DonationManagmentServer.Models.User> User { get; set; } = default!;
        //public DbSet<DonationManagmentServer.Models.Donor> Donor { get; set; } = default!;
        //public DbSet<DonationManagmentServer.Models.Donation> Donation { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הגדרת מפתחות ראשיים
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Donor>().HasKey(d => d.DonorId);
            modelBuilder.Entity<Donation>().HasKey(d => d.DonationId);
            modelBuilder.Entity<FileS3>().HasKey(f => f.FileId);
            modelBuilder.Entity<Receipt>().HasKey(r => r.ReceiptID);


            modelBuilder.Entity<Donor>()
                .HasOne(d => d.User)
                .WithMany(u => u.Donors)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany(dn => dn.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileS3>()
                .HasOne(f => f.User)
                .WithMany(u => u.Files)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Donation)
                .WithMany(d => d.Receipts)
                .HasForeignKey(r => r.DonationID);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.FileS3)
                .WithMany(f => f.Receipts)
                .HasForeignKey(r => r.FileID);

        }

    }
}
