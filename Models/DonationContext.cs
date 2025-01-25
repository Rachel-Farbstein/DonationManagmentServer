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
        public DbSet<FileDetails> FileDetails { get; set; } = default!;
        public DbSet<Receipt> Receipt { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הגדרת מפתחות ראשיים
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Donor>().HasKey(d => d.DonorId);
            modelBuilder.Entity<Donation>().HasKey(d => d.DonationId);
            modelBuilder.Entity<FileDetails>().HasKey(f => f.FileId);
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

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.FileDetails)
                .WithMany()
       .         HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileDetails>()
                .HasOne(f => f.User);
            //.WithMany(u => u.Files)
            //.HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Donation)
                .WithMany(d => d.Receipts)
                .HasForeignKey(r => r.DonationID);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.FileDetails)
                .WithMany(f => f.Receipts)
                .HasForeignKey(r => r.FileID);

        }

    }
}
