using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Repisotories
{
    public class DonationRepisotory
    {
        private readonly DonationContext _dbContext;

        public DonationRepisotory(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Donation>> GetDonationsAsync(int userId)
        {
            return await _dbContext.Donations.Where(d => d.UserId == userId).ToListAsync();
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

            var donationList = _dbContext.Donations.Where(d => donationIds.Contains(d.DonorId)).ToList();
            if (donationList.Any())
            {
                _dbContext.Donations.RemoveRange(donationList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

