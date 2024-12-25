using System;
using DonationManagmentServer.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Repisotories
{
    public class UserRepository
    {
        private readonly DonationContext _dbContext;

        public UserRepository(DonationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers() {
            return await _dbContext.User.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<User?> GetUserBycognitoIdAsync(string cognitoUserId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.CognitoUserId == cognitoUserId);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                _dbContext.User.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
