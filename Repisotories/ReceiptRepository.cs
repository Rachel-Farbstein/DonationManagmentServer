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

        public async Task<IEnumerable<Receipt>> GetReceipts(int userId)
        {
            return await _dbContext.Receipt
              .Where(receipt => receipt.Donation.Donor.UserId == userId).ToListAsync();
        }

        public async Task AddReceiptAsync(Receipt receipt, FileS3 fileS3)
        {

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // שמירת קובץ
                _dbContext.File.Add(fileS3);
                await _dbContext.SaveChangesAsync();

                // שמירת קבלה
                receipt.FileID = fileS3.FileId;
                _dbContext.Receipt.Add(receipt);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Receipt?> GetReceiptByIdAsync(int receiptId)
        {
            return await _dbContext.Receipt.FirstOrDefaultAsync(r => r.ReceiptID == receiptId);
        }

        public async Task UpdateReceiptAsync(Receipt receipt)
        {
            _dbContext.Receipt.Update(receipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteReceiptAsync(int receiptID)
        {

            var receipt = await _dbContext.Receipt
                .Include(r => r.FileS3)
                .FirstOrDefaultAsync(r => r.ReceiptID == receiptID);

            if (receipt != null)
            {
                if (receipt.FileS3 != null)
                {
                    _dbContext.File.Remove(receipt.FileS3);
                }
                _dbContext.Receipt.Remove(receipt);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteReceiptsAsync(List<int> receiptIds)
        {

            var receiptList = _dbContext.Receipt
                .Where(r => receiptIds.Contains(r.ReceiptID))
                .Include(r => r.FileS3)
                .ToList();
            if (receiptList.Any())
            {
                var fileToRemove = receiptList.Select(r => r.FileS3).ToList();
                if (!fileToRemove.Any())
                {
                    _dbContext.File.RemoveRange(fileToRemove);
                }

                _dbContext.Receipt.RemoveRange(receiptList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

