using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Repisotories
{
    public class FileS3Repository
    {
        private readonly DonationContext _dbContext;

        public FileS3Repository(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FileS3>> GetFiles(int userId)
        {
            return await _dbContext.File.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<FileS3?> GetFileByIdAsync(int fileId)
        {
            return await _dbContext.File.FirstOrDefaultAsync(f => f.FileId == fileId);
        }

        public async Task AddFileAsync(FileS3 fileS3)
        {
            _dbContext.File.Add(fileS3);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFileAsync(FileS3 fileS3)
        {
            _dbContext.File.Update(fileS3);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _dbContext.File.FirstOrDefaultAsync(f => f.FileId == fileId);
            if (file != null)
            {
                _dbContext.File.Remove(file);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteFilesAsync(List<int> fileIds)
        {

            var fileList = _dbContext.File.Where(f => fileIds.Contains(f.FileId)).ToList();
            if (fileList.Any())
            {
                _dbContext.File.RemoveRange(fileList);
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}

