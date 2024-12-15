using System;
using DonationManagmentServer.Models;
using DonationManagmentServer.Repisotories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Services
{
    public class DonorService
    {

        private readonly DonorRepository _donorRepository;
        private readonly UserService _userService;
        public DonorService(DonorRepository donorRepository, UserService userService)
        {
            _donorRepository = donorRepository;
            _userService = userService;
        }

        public async Task AddDonorAsync(Donor donor)
        {
            await _donorRepository.AddDonorAsync(donor);
        }

        public async Task<IEnumerable<Donor>> GetDonors(int userId)
        {
            return await _donorRepository.GetDonors(userId);
        }

        public async Task<Donor?> GetDonorByIdAsync(int donorId)
        {
            return await _donorRepository.GetDonorByIdAsync(donorId);
        }


        public async Task UpdateDonorAsync(Donor donor)
        {
            await _donorRepository.UpdateDonorAsync(donor);
        }

        public async Task DeleteDonorAsync(int donorId)
        {
            await _donorRepository.DeleteDonorAsync(donorId);
        }

        public async Task DeleteDonorsAsync(List<int> donorIds)
        {
            await _donorRepository.DeleteDonorsAsync(donorIds);
        }

    }

}
