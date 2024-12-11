using System;
using System.Security.Claims;
using DonationManagmentServer.Models;
using DonationManagmentServer.Repisotories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Services
{
    public class UserService
    {

        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> getUserIdByToken(ClaimsPrincipal user)
        {
            var cognitoUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(cognitoUserId))
            {
                throw new Exception("User not Found");
            }

            var u = await _userRepository.GetUserByIdAsync(cognitoUserId);
            if (u == null)
            {
                throw new Exception("User not Found");
            }

            return u.Id;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(string cognitoUserId)
        {
            return await _userRepository.GetUserByIdAsync(cognitoUserId);
        }


        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.CognitoUserId);
            if (existingUser != null)
            {
                await _userRepository.UpdateUserAsync(existingUser);
            }
            else
            {
                await this.AddUserAsync(user);
            }
        }

        public async Task DeleteUserAsync(string cognitoUserId)
        {
            await _userRepository.DeleteUserAsync(cognitoUserId);
        }

    }

}
