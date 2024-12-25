using System;
using System.Net;
using AutoMapper;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Repisotories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DonationManagmentServer.Services
{
    public class DonorService
    {

        private readonly DonorRepository _donorRepository;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public DonorService(DonorRepository donorRepository, 
                            UserService userService,
                            IMapper mapper)
        {
            _donorRepository = donorRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DonorDto>> GetDonors(int userId)
        {
            var donors = await _donorRepository.GetDonors(userId);
            return donors.Select(d => ConvertDonorToDonorDto(d)).ToList();
        }

        public async Task<DonorDto?> GetDonorByIdAsync(int donorId)
        {
            var d =  await _donorRepository.GetDonorByIdAsync(donorId);
            if (d == null) 
                return null;
            return ConvertDonorToDonorDto(d);
        }

        public async Task AddDonorAsync(DonorDto donorDto, int userId)
        {
            var donor = ConvertDonorDtoToDonor(donorDto);
            donor.UserId = userId;
            await _donorRepository.AddDonorAsync(donor);
        }

        public async Task UpdateDonorAsync(DonorDto donorDto, int userId)
        {
            var donor = ConvertDonorDtoToDonor(donorDto);
            donor.UserId = userId;
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

        private DonorDto ConvertDonorToDonorDto(Donor donor) {
            return _mapper.Map<DonorDto>(donor);
        }
        private Donor ConvertDonorDtoToDonor(DonorDto donorDto)
        {
            return _mapper.Map<Donor>(donorDto);
        }

    }

}
