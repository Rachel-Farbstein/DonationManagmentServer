using System;
using System.Drawing;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public DonationService(
            DonationRepository donationRepository, 
            UserService userService,
            IMapper mapper )
        {
            _donationRepository = donationRepository;
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<IEnumerable<DonationDto>> GetDonations(int userId)
        {
       
            var donations = await _donationRepository.GetDonationsAsync(userId);
            return donations.Select(d => ConvertDonationToDonationDto(d)).ToList();
        }

        public IEnumerable<DonationDtoWithDonorName> GetDonationsWithDonorName(int userId)
        {
            var donationsWithDonorName =  _donationRepository.GetDonationsWithDonorName(userId);
            return donationsWithDonorName.Select(d => _mapper.Map<DonationDtoWithDonorName>(d));
        }

        public async Task<DonationDto?> GetDonationByIdAsync(int donationID)
        {
            var donation =  await _donationRepository.GetDonationByIdAsync(donationID);
            return ConvertDonationToDonationDto(donation);
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsByDonorIdAsync(int donorId)
        {
            var donations = await _donationRepository.GetDonationsByDonorIdAsync(donorId);
            return donations.Select(d => ConvertDonationToDonationDto(d)).ToList();
        }

        public async Task AddDonationAsync(DonationDto donationDto)
        {
            var donation = ConvertDonationDtoToDonation(donationDto);
            await _donationRepository.AddDonationAsync(donation);
        }
        public async Task UpdateDonationAsync(DonationDto donationDto)
        {
            var donation = ConvertDonationDtoToDonation(donationDto);
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

        private DonationDto ConvertDonationToDonationDto(Donation? donation)
        {
            return _mapper.Map<DonationDto>(donation);
        }
        private Donation ConvertDonationDtoToDonation(DonationDto donationDto)
        {
            return _mapper.Map<Donation>(donationDto);
        }
    }

}
