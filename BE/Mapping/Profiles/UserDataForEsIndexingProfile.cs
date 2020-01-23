using System.Linq;
using AutoMapper;
using BE.Dtos;
using BE.Models;

namespace BE
{
    public class UserDataForEsIndexingProfile : Profile
    {
        public UserDataForEsIndexingProfile()
        {
            CreateMap<User, UserForIndexingDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.GenderId))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.EducationId, opt => opt.MapFrom(src => src.EducationId))
                .ForMember(dest => dest.MaritalStatusId,
                    opt => opt.MapFrom(src => src.AdditionalInfo.MaritalStatusId))
                .ForMember(dest => dest.ReligionId, opt => opt.MapFrom(src => src
                    .AdditionalInfo.ReligionId))
                .ForMember(dest => dest.AlcoholAttitudeId, opt => opt.MapFrom(src => src
                    .AdditionalInfo.AlcoholAttitudeId))
                .ForMember(dest => dest.SmokingAttitudeId, opt => opt.MapFrom(src => src
                    .AdditionalInfo.SmokingAttitudeId))
                .ForMember(dest => dest.UserInterests, opt => opt.MapFrom(src => src
                    .UserInterests.Select(e => new UserInterestForElasticsearchDto
                    {
                        Id = e.Id,
                        Title = e.Interest.Title
                    })));
        }
    }
}