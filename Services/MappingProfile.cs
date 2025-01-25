using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Models;
using AutoMapper;

namespace DonationManagmentServer.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Donor, DonorDto>().ReverseMap();
            CreateMap<Donation, DonationDto>().ReverseMap();
            CreateMap<DonationWithDonorName, DonationDtoWithDonorName>().ReverseMap();
            CreateMap<Receipt, ReceiptDto>().ReverseMap();
            CreateMap<FileDetails,FileDetailsDto>().ReverseMap();
            CreateMap<ReceiptWithFile, ReceiptWithFileDto>().ReverseMap();
        }
    }
}


