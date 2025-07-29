using AutoMapper;
using ApptManager.Models;
using ApptManager.DTOs;

namespace ApptManager.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<SlotGenerationRequestDto, Slot>();

            CreateMap<CreateBookingDto, Bookings>()
                .ForMember(dest => dest.BookedOn, opt => opt.MapFrom(_ => DateTime.UtcNow));

            
            CreateMap<User, LoginResponseDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateUserDto, User>()
                 .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => Enum.Parse<UserType>(src.UserType, true)));

            CreateMap<CreateTaxProfessionalDto, TaxProfessional>();
            CreateMap<TaxProfessional, TaxProfessionalDto>();

            CreateMap<Slot, SlotDto>();
            CreateMap<SlotUpdateDto, Slot>();




            CreateMap<Bookings, BookingDetailsDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) 
                .ForMember(dest => dest.TaxProfessionalName, opt => opt.Ignore()) 
                .ForMember(dest => dest.StartTime, opt => opt.Ignore())
                .ForMember(dest => dest.EndTime, opt => opt.Ignore());
        }
    }
}
