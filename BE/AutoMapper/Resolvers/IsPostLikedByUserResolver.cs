using System;
using System.Linq;
using AutoMapper;
using BE.Dtos;
using BE.Models;

namespace BE.AutoMapper.Resolvers
{
    public class IsPostLikedByUserResolver: IValueResolver<UserPost, UserPostDto, bool>
    {
        public bool Resolve(UserPost source, UserPostDto destination, bool member, ResolutionContext context)
        {
            return source.Post.PostLike
                .ToList()
                .Exists(like => like.PostId == source.PostId && like.UserId == Convert.ToInt32(context.Items["userId"]));
        }
    }
}