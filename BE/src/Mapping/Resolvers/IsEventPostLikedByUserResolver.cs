using System;
using System.Linq;
using AutoMapper;
using BE.Dtos.PostDtos;
using BE.Models;

namespace BE.Mapping.Resolvers
{
    public class
        IsEventPostLikedByUserResolver : IValueResolver<EventPost, EventPostDto, bool>
    {
        public bool Resolve(EventPost source, EventPostDto destination, bool member,
            ResolutionContext context)
        {
            if (context.Items["userId"] != null)
            {
                var userId = Convert.ToInt32(context.Items["userId"]);
                return source.Post.PostLike
                    .ToList()
                    .Exists(like =>
                        like.PostId == source.PostId && like.UserId == userId);
            }

            return false;
        }
    }
}