using System;
using System.Drawing;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Repisotories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DonationManagmentServer.Services
{
    public class DonationService
    {

        private readonly DonationRepository _donationRepository;
        private readonly UserService _userService;
        public DonationService(DonationRepository donationRepository, UserService userService)
        {
            _donationRepository = donationRepository;
            _userService = userService;
        }


        public async Task<IEnumerable<Donation>> GetDonations(int userId)
        {
            return await _donationRepository.GetDonationsAsync(userId);
        }

        public IEnumerable<DonationWithDonorNameDto> GetDonationsWithDonorName(int userId)
        {
            return _donationRepository.GetDonationsWithDonorName(userId);
        }

        public async Task<Donation?> GetDonationByIdAsync(int donationID)
        {
            return await _donationRepository.GetDonationByIdAsync(donationID);
        }

        public async Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(int donorId)
        {
            return await _donationRepository.GetDonationsByDonorIdAsync(donorId);
        }

        public async Task AddDonationAsync(Donation donation)
        {
            await _donationRepository.AddDonationAsync(donation);
        }
        public async Task UpdateDonationAsync(Donation donation)
        {
            await _donationRepository.UpdateDonationAsync(donation);
        }

        public async Task DeleteDonationAsync(int donationId)
        {
            await _donationRepository.DeleteDonationAsync(donationId);
        }

        public async Task DeleteDonationsAsync(List<int> donationIds)
        {
            await _donationRepository.DeleteDonationsAsync(donationIds);
        }

    }

}
