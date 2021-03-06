using AutoMapper;
using BE.Features.Post.Dtos;
using BE.Mapping.Resolvers;
using BE.Models;

namespace BE.Mapping.Profiles
{
    public class UserPostProfile : Profile
    {
        public UserPostProfile()
        {
            CreateMap<UserPost, UserPostDto>()
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Post.Content))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Post.Date))
                .ForMember(dest => dest.ImagePath,
                    opt => opt.MapFrom(src => src.Post.ImagePath))
                .ForMember(dest => dest.CommentsCount,
                    opt => opt.MapFrom(src => src.Post.Comment.Count))
                .ForMember(dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Post.PostLike.Count))
                .ForMember(dest => dest.IsPostLikedByUser,
                    opt => opt.MapFrom(new IsUserPostLikedByUserResolver()))
                .ForMember(dest => dest.Avatar,
                    opt => opt.MapFrom(src => src.User.Avatar));
        }
    }
}