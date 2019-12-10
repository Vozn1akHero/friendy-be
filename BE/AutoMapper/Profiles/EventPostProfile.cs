using AutoMapper;
using BE.AutoMapper.Resolvers;
using BE.Dtos;
using BE.Models;

namespace BE
{
    public class EventPostProfile: Profile
    {
        public EventPostProfile()
        {
            CreateMap<EventPost, EventPostDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Post.Content))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Post.Date))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Post.ImagePath))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Post.Comment.Count))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Post.PostLike.Count))
                .ForMember(dest => dest.IsPostLikedByUser, opt => opt.MapFrom(new IsEventPostLikedByUserResolver()));
        }
    }
}