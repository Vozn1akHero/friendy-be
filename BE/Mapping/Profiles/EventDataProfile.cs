using AutoMapper;
using BE.Dtos.EventDtos;
using BE.Models;

namespace BE
{
    public class EventDataProfile: Profile
    {
        public EventDataProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.StreetNumber, opt => opt.MapFrom(src => src.StreetNumber))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.AvatarPath, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.BackgroundPath, opt => opt.MapFrom(src => src.Background))
                .ForMember(dest => dest.ParticipantsAmount, opt => opt.MapFrom(src => src.ParticipantsAmount))
                .ForMember(dest => dest.CurrentParticipantsAmount, opt => opt.MapFrom(src => src.EventParticipants.Count));
        }
    }
}