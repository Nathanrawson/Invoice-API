using AutoMapper;
using Invoice.Api.DtoModels;
using Invoice.DomainModels;

namespace Invoice.Api.MapperProfiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoiceDto, DomainModels.Invoice>()
                .ForMember(dest => dest.ClientAddress, opt => opt.MapFrom(src => src.ClientAddress))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.CompanyContact, opt => opt.MapFrom(src => src.CompanyContact))
                .ForMember(dest => dest.BankingDetails, opt => opt.MapFrom(src => src.BankingDetails))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(src => src.LineItems)).ReverseMap();

            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<BankingDetailsDto, BankingDetails>().ReverseMap();
            CreateMap<ContactDto, Contact>().ReverseMap();
            CreateMap<InvoiceLineItemDto, InvoiceLineItem>().ReverseMap();
        }
    }
}
