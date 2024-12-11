using System;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Repisotories
{
    public class DonorRepository
    {
        private readonly DonationContext _dbContext;

        public DonorRepository(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Donor>> GetDonors(int userId)
        {
            return await _dbContext.Donor.Where(d => d.UserId == userId).ToListAsync();
        }

        public async Task AddDonorAsync(Donor donor)
        {
            _dbContext.Donor.Add(donor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Donor?> GetDonorByIdAsync(int id)
        {
            return await _dbContext.Donor.FirstOrDefaultAsync(d => d.DonorId == id);
        }

        public async Task UpdateDonorAsync(Donor donor)
        {
            _dbContext.Donor.Update(donor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDonorAsync(int id)
        {
            var donor = await _dbContext.Donor.FirstOrDefaultAsync(d => d.DonorId == id);
            if (donor != null)
            {
                _dbContext.Donor.Remove(donor);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}

