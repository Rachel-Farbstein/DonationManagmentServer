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
        private readonly S3Service _s3Service;
        public DonationService( DonationRepository donationRepository, 
                                UserService userService,
                                IMapper mapper,
                                S3Service s3Service)
        {
            _donationRepository = donationRepository;
            _userService = userService;
            _mapper = mapper;
            _s3Service = s3Service;
        }


        public async Task<IEnumerable<DonationDto>> GetDonations(int userId)
        {
       
            var donations = await _donationRepository.GetDonationsAsync(userId);
            return donations.Select(d => ConvertDonationToDonationDto(d)).ToList();
        }

        public IEnumerable<DonationDtoWithDonorName> GetDonationsWithDonorName(int userId)
        {
            var donationsWithDonorName =  _donationRepository.GetDonationsWithDonorName(userId);
            return donationsWithDonorName.Select(d => new DonationDtoWithDonorName
            {
                Donation = _mapper.Map<DonationDto>(d.Donation),
                DonorName = d.DonorName,
                FileDetails = _mapper.Map<FileDetailsDto>(d.Donation?.FileDetails)

            });
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

        public async Task<IEnumerable<TotalAmountMonth>> GetDonationsAmtByMonth(int userId)
        {
            var donations = await _donationRepository.GetDonationsAmtByMonth(userId);
            return donations;
        }

        public async Task<IEnumerable<DonorTotalAmount>> GetDonationsAmtByDonors(int userId)
        {
            var donorsAndAmt = await _donationRepository.GetDonationsAmtByDonors(userId);
            return donorsAndAmt;
        }

        public async Task AddDonationAsync(DonationDto donationDto)
        {
            var donation = ConvertDonationDtoToDonation(donationDto);
            await _donationRepository.AddDonationAsync(donation);
        }

        public async Task<FileDetailsDto> AddFileToDonation(int donationId, IFormFile file, int userId, string congitoUserId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            try
            {
                var fileS3Dto = _s3Service.UploadFileAsync(congitoUserId, file);

                var fileDeatils = new FileDetails
                {
                    UserId = userId,
                    FileName = file.FileName,
                    S3FileKey = fileS3Dto.Result.S3FileKey,
                    S3FileUrl = fileS3Dto.Result.S3FileUrl,
                    S3BucketName = fileS3Dto.Result.S3BucketName,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    UploadedAt = DateTime.UtcNow,
                    IsDeleted = false,
                };

                await _donationRepository.AddFileToDonation(donationId, fileDeatils);
                return _mapper.Map<FileDetailsDto>(fileDeatils);
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task DeleteDonationFile(int donationId)
        {
            await _donationRepository.DeleteDonationFile(donationId);
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
