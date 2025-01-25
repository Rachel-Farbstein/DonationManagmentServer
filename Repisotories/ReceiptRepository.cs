using System;
using System.Drawing;
using AutoMapper;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace DonationManagmentServer.Repisotories
{
    public class ReceiptRepository
    {
        private readonly DonationContext _dbContext;
        private readonly IMapper _mapper;
        public ReceiptRepository(DonationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Receipt>> GetReceipts(int userId)
        {
            return await _dbContext.Receipt
              .Where(receipt => receipt.Donation.Donor.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<ReceiptWithFile>> GetReceiptsWithFiles(int userId)
        {
            var receipts = await _dbContext.Receipt
                .Join(_dbContext.FileDetails,
                      receipt => receipt.FileID,
                      fileDetails => fileDetails.FileId,
                      (receipt, fileDetails) => new ReceiptWithFile
                      {
                          Receipt = receipt,
                          FileDetails = fileDetails,
                      })
                .Where(joined => joined.Receipt.Donation.Donor.UserId == userId).ToListAsync();

            return receipts;

        }

        public async Task AddReceiptAsync(Receipt receipt, FileDetails fileDetails)
        {

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // שמירת קובץ
                _dbContext.FileDetails.Add(fileDetails);
                await _dbContext.SaveChangesAsync();

                // שמירת קבלה
                receipt.FileID = fileDetails.FileId;
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

        public async Task<FileDetails?> GetFileDetailsByReceiptId(int receiptId)
        {
            var rec =  await _dbContext.Receipt.Include(r => r.FileDetails)
                                           .FirstOrDefaultAsync(r => r.ReceiptID == receiptId);
            return rec?.FileDetails;
        }

        public async Task UpdateReceiptAsync(Receipt receipt)
        {
            _dbContext.Receipt.Update(receipt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteReceiptAsync(int receiptID)
        {

            var receipt = await _dbContext.Receipt
                .Include(r => r.FileDetails)
                .FirstOrDefaultAsync(r => r.ReceiptID == receiptID);

            if (receipt != null)
            {
                if (receipt.FileDetails != null)
                {
                    _dbContext.FileDetails.Remove(receipt.FileDetails);
                }
                _dbContext.Receipt.Remove(receipt);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteReceiptsAsync(List<int> receiptIds)
        {

            var receiptList = _dbContext.Receipt
                .Where(r => receiptIds.Contains(r.ReceiptID))
                .Include(r => r.FileDetails)
                .ToList();
            if (receiptList.Any())
            {
                var fileToRemove = receiptList.Select(r => r.FileDetails).ToList();
                if (!fileToRemove.Any())
                {
                    _dbContext.FileDetails.RemoveRange(fileToRemove);
                }

                _dbContext.Receipt.RemoveRange(receiptList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

