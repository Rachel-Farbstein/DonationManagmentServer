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
            return await _dbContext.Donations
                .Where(donation => donation.Donor.UserId == userId).ToListAsync();
        }

        public IEnumerable<DonationWithDonorNameDto> GetDonationsWithDonorName(int userId)
        {
            var donations = _dbContext.Donations
                .Join(_dbContext.Donors,
                donation => donation.DonorId,
                donor => donor.DonorId,
                (donation, donor) => new { donation, donor })
                .Where(joined => joined.donor.UserId == userId)
                .Select(joined => new DonationWithDonorNameDto
                {
                    Donation = joined.donation, 
                    DonorName = joined.donor.FullName
                }).ToList();

            return donations;

        }

        public async Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(int donorId)
        {
            return await _dbContext.Donations.Where(d => d.DonorId == donorId).ToListAsync();
        }

        public async Task<Donation?> GetDonationByIdAsync(int donationId)
        {
            return await _dbContext.Donations.FirstOrDefaultAsync(d => d.DonationId == donationId);
        }

        public async Task AddDonationAsync(Donation donation)
        {
            _dbContext.Donations.Add(donation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDonationAsync(Donation donation)
        {
            _dbContext.Donations.Update(donation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDonationAsync(int donationId)
        {
            var donation = await _dbContext.Donations.FirstOrDefaultAsync(d => d.DonationId == donationId);
            if (donation != null)
            {
                _dbContext.Donations.Remove(donation);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDonationsAsync(List<int> donationIds)
        {

            var donationList = _dbContext.Donations.Where(d => donationIds.Contains(d.DonationId)).ToList();
            if (donationList.Any())
            {
                _dbContext.Donations.RemoveRange(donationList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

