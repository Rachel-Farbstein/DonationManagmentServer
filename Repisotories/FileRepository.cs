using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Repisotories
{
    public class FileRepository
    {
        private readonly DonationContext _dbContext;

        public FileRepository(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FileDetails>> GetFiles(int userId)
        {
            return await _dbContext.FileDetails.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<FileDetails?> GetFileByIdAsync(int fileId)
        {
            return await _dbContext.FileDetails.FirstOrDefaultAsync(f => f.FileId == fileId);
        }

        public async Task AddFileAsync(FileDetails fileDetails)
        {
            _dbContext.FileDetails.Add(fileDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFileAsync(FileDetails fileDetails)
        {
            _dbContext.FileDetails.Update(fileDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _dbContext.FileDetails.FirstOrDefaultAsync(f => f.FileId == fileId);
            if (file != null)
            {
                _dbContext.FileDetails.Remove(file);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteFilesAsync(List<int> fileIds)
        {

            var fileList = _dbContext.FileDetails.Where(f => fileIds.Contains(f.FileId)).ToList();
            if (fileList.Any())
            {
                _dbContext.FileDetails.RemoveRange(fileList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

