using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace DonationManagmentServer.Repisotories
{
    public class DonationRepository
    {
        private readonly DonationContext _dbContext;

        public DonationRepository(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Donation>> GetDonationsAsync(int userId)
        {
            return await _dbContext.Donation
                .Where(donation => donation.Donor.UserId == userId).ToListAsync();
        }

        public IEnumerable<DonationWithDonorName> GetDonationsWithDonorName(int userId)
        {
            var donations = _dbContext.Donation
                .Join(_dbContext.Donor,
                donation => donation.DonorId,
                donor => donor.DonorId,
                (donation, donor) => new { donation, donor })
                .Where(joined => joined.donor.UserId == userId)
                .Select(joined => new DonationWithDonorName
                {
                    Donation = joined.donation, 
                    DonorName = joined.donor.FullName
                }).ToList();

            return donations;

        }

        public async Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(int donorId)
        {
            return await _dbContext.Donation.Where(d => d.DonorId == donorId).ToListAsync();
        }

        public async Task<Donation?> GetDonationByIdAsync(int donationId)
        {
            return await _dbContext.Donation.FirstOrDefaultAsync(d => d.DonationId == donationId);
        }

        public async Task AddDonationAsync(Donation donation)
        {
            _dbContext.Donation.Add(donation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDonationAsync(Donation donation)
        {
            _dbContext.Donation.Update(donation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDonationAsync(int donationId)
        {
            var donation = await _dbContext.Donation.FirstOrDefaultAsync(d => d.DonationId == donationId);
            if (donation != null)
            {
                _dbContext.Donation.Remove(donation);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDonationsAsync(List<int> donationIds)
        {

            var donationList = _dbContext.Donation.Where(d => donationIds.Contains(d.DonationId)).ToList();
            if (donationList.Any())
            {
                _dbContext.Donation.RemoveRange(donationList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

