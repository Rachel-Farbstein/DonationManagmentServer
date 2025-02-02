using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Services;
using Microsoft.AspNetCore.Mvc;
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
                .Include(d => d.FileDetails)
                //.Include(d => d.Donor)
                .Where(d => d.Donor.UserId == userId)
                .Select(d => new DonationWithDonorName
                {
                    Donation = d,
                    DonorName = d.Donor.FullName,
                    //FileDetails = d.FileDetails
                }).ToList();

            //var donations = _dbContext.Donation
            //    .Include(d => d.FileDetails)
            //    .Join(_dbContext.Donor,
            //    donation => donation.DonorId,
            //    donor => donor.DonorId,
            //    (donation, donor) => new { donation, donor })
            //    .Where(joined => joined.donor.UserId == userId)
            //    .Select(joined => new DonationWithDonorName
            //    {
            //        Donation = joined.donation,
            //        DonorName = joined.donor.FullName,
            //        FileDetails = joined.donation.FileDetails,
            //    }).ToList();

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

        public async Task<IEnumerable<TotalAmountMonth>> GetDonationsAmtByMonth(int userId)
        {
            try
            {
                var last12Months = Enumerable.Range(0, 12)
                    .Select(i => DateTime.UtcNow.AddMonths(-i))
                    .Select(d => new TotalAmountMonth
                    {
                        MonthYear = $"{d.Year}-{d.Month:D2}",
                        TotalAmount = 0 // ברירת מחדל - אם אין תרומות, הסכום יהיה 0
                    })
                    .OrderBy(d => d.MonthYear)
                    .ToList();

                var donations = _dbContext.Donation
                    .Where(donation => donation.Donor.UserId == userId
                           && donation.DonationDate >= DateTime.UtcNow.AddMonths(-12))
                    .GroupBy(d => new { Year = d.DonationDate.Year, Month = d.DonationDate.Month })
                     .AsEnumerable()
                     .Select(g => new TotalAmountMonth
                     {
                         MonthYear = $"{g.Key.Year}-{g.Key.Month:D2}",
                         TotalAmount = g.Sum(d => d.Amount)
                     })
                    .OrderBy(d => d.MonthYear).ToList();

                foreach (var month in last12Months)
                {
                    var found = donations.FirstOrDefault(d => d.MonthYear == month.MonthYear);
                    if (found != null)
                    {
                        month.TotalAmount = found.TotalAmount;
                    }
                }

                return last12Months;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<DonorTotalAmount>> GetDonationsAmtByDonors(int userId)
        {
            try
            {
                var donorsAndAmt = await _dbContext.Donor
                    .Include(d => d.Donations)
                    .Select(d => new DonorTotalAmount
                    {
                        DonorId = d.DonorId,
                        DonorName = d.FullName,
                        TotalAmount = d.Donations.Sum(d => d.Amount)
                    })
                     .OrderByDescending(d => d.TotalAmount)
                     .ToListAsync();

                return donorsAndAmt;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task AddDonationAsync(Donation donation)
        {
            _dbContext.Donation.Add(donation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddFileToDonation(int donationId, FileDetails fileDetails)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {

                var donation = await _dbContext.Donation.FirstOrDefaultAsync(d => d.DonationId == donationId);
                if (donation == null)
                {
                    throw new Exception("Donation not found");
                }
                if (donation.FileId != null)
                {
                    var fileDet = _dbContext.FileDetails.FirstOrDefault(f => f.FileId == donation.FileId);
                    if (fileDet != null)
                    {
                        fileDet.IsDeleted = true;
                        _dbContext.FileDetails.Update(fileDet);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // שמירת קובץ
                _dbContext.FileDetails.Add(fileDetails);
                await _dbContext.SaveChangesAsync();

                donation.FileId = fileDetails.FileId;
                _dbContext.Donation.Update(donation);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateDonationAsync(Donation donation)
        {
            _dbContext.Donation.Update(donation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDonationAsync(int donationId)
        {
            var donation = await _dbContext.Donation
                .Include(d => d.FileDetails)
                .FirstOrDefaultAsync(d => d.DonationId == donationId);
            if (donation != null)
            {
                try
                {
                    if (donation.FileDetails != null)
                    {
                        donation.FileDetails.IsDeleted = true;
                        _dbContext.FileDetails.Update(donation.FileDetails);
                    }

                    _dbContext.Donation.Remove(donation);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
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

        public async Task DeleteDonationFile(int donationId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {

                var donation = await _dbContext.Donation.FirstOrDefaultAsync(d => d.DonationId == donationId);
                if (donation == null)
                {
                    throw new Exception("Donation not found");
                }
                if (donation.FileId != null)
                {
                    var fileDet = _dbContext.FileDetails.FirstOrDefault(f => f.FileId == donation.FileId);
                    if (fileDet != null)
                    {
                        fileDet.IsDeleted = true;
                        _dbContext.FileDetails.Update(fileDet);
                        await _dbContext.SaveChangesAsync();
                    }

                    donation.FileId = null;
                    _dbContext.Donation.Update(donation);
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


    }
}

