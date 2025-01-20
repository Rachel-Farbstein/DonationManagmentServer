using System;
using System.Net;
using Amazon.Runtime.Internal;
using Amazon.S3;
using AutoMapper;
using Azure.Core;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Repisotories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DonationManagmentServer.Services
{
    public class ReceiptService
    {

        private readonly ReceiptRepository _receiptRepository;
        private readonly FileRepository _fileRepository;
        private readonly S3Service _s3Service;
        private readonly IMapper _mapper;

        public ReceiptService(ReceiptRepository receiptRepository,
                              FileRepository fileRepository,
                              S3Service s3Service,
                              IMapper mapper)
        {
            _receiptRepository = receiptRepository;
            _fileRepository = fileRepository;
            _s3Service = s3Service;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptDto>> GetReceipts(int userId)
        {
            var receipts = await _receiptRepository.GetReceipts(userId);
            return receipts.Select(r => _mapper.Map<ReceiptDto>(r)).ToList();
        }

        //public async Task<DonorDto?> GetDonorByIdAsync(int donorId)
        //{
        //    var d =  await _donorRepository.GetDonorByIdAsync(donorId);
        //    if (d == null) 
        //        return null;
        //    return ConvertDonorToDonorDto(d);
        //}

        public async Task AddReceiptAsync(ReceiptDto receiptDto, int userId, string congitoUserId)
        {
            if (receiptDto.File == null || receiptDto.File.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            try
            {
                var fileS3Dto = _s3Service.UploadFileAsync(congitoUserId, receiptDto.File);

                var fileS3 = new FileS3
                {
                    UserId = userId,
                    FileName = receiptDto.File.FileName,
                    S3FileKey = fileS3Dto.Result.UniqueKey,
                    S3FileUrl = fileS3Dto.Result.FileUrl,
                    S3BucketName = fileS3Dto.Result.BucketName,
                    ContentType = receiptDto.File.ContentType,
                    FileSize = receiptDto.File.Length,
                    UploadedAt = DateTime.UtcNow,
                    IsDeleted = false,
                };

                //await _fileRepository.AddFileAsync(fileS3);
                var receipt = _mapper.Map<Receipt>(receiptDto);
                await _receiptRepository.AddReceiptAsync(receipt , fileS3);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task UpdateDonorAsync(DonorDto donorDto, int userId)
        //{
        //    var donor = ConvertDonorDtoToDonor(donorDto);
        //    donor.UserId = userId;
        //    await _donorRepository.UpdateDonorAsync(donor);
        //}

        //public async Task DeleteDonorAsync(int donorId)
        //{
        //    await _donorRepository.DeleteDonorAsync(donorId);
        //}

        //public async Task DeleteDonorsAsync(List<int> donorIds)
        //{
        //    await _donorRepository.DeleteDonorsAsync(donorIds);
        //}

        private DonorDto ConvertDonorToDonorDto(Donor donor)
        {
            return _mapper.Map<DonorDto>(donor);
        }
        private Donor ConvertDonorDtoToDonor(DonorDto donorDto)
        {
            return _mapper.Map<Donor>(donorDto);
        }

    }

}
