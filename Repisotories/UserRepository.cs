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
            return await _dbContext.Users.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(string cognitoUserId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.CognitoUserId == cognitoUserId);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string cognitoUserId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.CognitoUserId == cognitoUserId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
