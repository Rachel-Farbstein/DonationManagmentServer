using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace DonationManagmentServer.Repisotories
{
    public class ReceiptRepository
    {
        private readonly DonationContext _dbContext;

        public ReceiptRepository(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Donor>> GetDonors(int userId)
        {
            return await _dbContext.Donor.Where(d => d.UserId == userId).ToListAsync();
        }

        public async Task AddDonorAsync(Donor donor)
        {
           var newDonor = _dbContext.Donor.Add(donor);
           await _dbContext.SaveChangesAsync();
        }

        public async Task<Donor?> GetDonorByIdAsync(int donorId)
        {
            return await _dbContext.Donor.FirstOrDefaultAsync(d => d.DonorId == donorId);
        }

        public async Task UpdateDonorAsync(Donor donor)
        {
            _dbContext.Donor.Update(donor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDonorAsync(int id)
        {

            var donor = await _dbContext.Donor
                .Include(d => d.Donations)
                .FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor != null)
            {
                if (donor.Donations != null)
                {
                    _dbContext.Donation.RemoveRange(donor.Donations);
                }
                _dbContext.Donor.Remove(donor);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDonorsAsync(List<int> donorIds)
        {

            var donorList = _dbContext.Donor
                .Where(d => donorIds.Contains(d.DonorId))
                .Include(d => d.Donations)
                .ToList();
            if (donorList.Any())
            {
                var donationsToRemove = donorList.SelectMany(d => d.Donations).ToList();
                if (!donationsToRemove.Any())
                {
                    _dbContext.Donation.RemoveRange(donationsToRemove);
                }

                _dbContext.Donor.RemoveRange(donorList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

