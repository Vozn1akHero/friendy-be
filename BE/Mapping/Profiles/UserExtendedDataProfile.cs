using AutoMapper;
using BE.AutoMapper.Resolvers;
using BE.Dtos;
using BE.Models;

namespace BE
{
    public class UserExtendedDataProfile : Profile
    {
        public UserExtendedDataProfile()
        {
            CreateMap<User, ExtendedUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EducationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MaritalStatusId, opt => opt.MapFrom(src => src.Id));
        }
    }
}