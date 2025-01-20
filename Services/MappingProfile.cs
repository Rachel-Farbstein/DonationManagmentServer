using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Models;
using AutoMapper;

namespace DonationManagmentServer.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Donor, DonorDto>();
            CreateMap<DonorDto, Donor>();
            CreateMap<Donation, DonationDto>();
            CreateMap<DonationDto, Donation>();
            CreateMap<DonationWithDonorName, DonationDtoWithDonorName>();
            CreateMap<DonationDtoWithDonorName, DonationWithDonorName>();
            CreateMap<Receipt, ReceiptDto>();
            CreateMap<ReceiptDto, Receipt>();
        }
    }
}


