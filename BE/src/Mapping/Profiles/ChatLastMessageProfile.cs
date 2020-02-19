using AutoMapper;
using BE.Dtos.ChatDtos.ServerDtos;
using BE.Models;

namespace BE.Mapping.Profiles
{
    public class ChatLastMessageProfileProfile : Profile
    {
        public ChatLastMessageProfileProfile()
        {
            CreateMap<ChatMessages, ChatLastMessageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatId))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Message.Content))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Message
                    .ImagePath))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Message.Date))
                /*.ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.Message
                .UserId))
                .ForMember(dest => dest.SenderAvatarPath, opt => opt.MapFrom(src => src
                .Message.User.Avatar))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src
                .Message.ReceiverId))
                .ForMember(dest => dest.ReceiverAvatarPath, opt => opt.MapFrom(src => 
                src.Message.Receiver.Avatar))
                .ForMember(dest => dest.InterlocutorId, opt => opt.MapFrom(new InterlocutorIdResolver()))
                .ForMember(dest => dest.InterlocutorAvatarPath, opt => opt.MapFrom(new InterlocutorAvatarResolver()))
                .ForMember(dest => dest.WrittenByRequestIssuer, opt => opt.MapFrom(new WrittenByRequestIssuerResolver()))*/
                ;
        }
    }
}