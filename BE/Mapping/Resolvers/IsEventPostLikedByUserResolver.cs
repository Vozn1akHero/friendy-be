using System;
using System.Linq;
using AutoMapper;
using BE.Dtos;
using BE.Models;

namespace BE.AutoMapper.Resolvers
{
    public class IsEventPostLikedByUserResolver: IValueResolver<EventPost, EventPostDto, bool>
    {
        public bool Resolve(EventPost source, EventPostDto destination, bool member, ResolutionContext context)
        {
            if (context.Items["userId"] != null)
            {
                int userId = Convert.ToInt32(context.Items["userId"]);
                return source.Post.PostLike
                    .ToList()
                    .Exists(like =>
                        like.PostId == source.PostId && like.UserId == userId);
            }
            return false;
        }
    }
}