using System;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Post.Dtos
{
    public class UserPostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int PostId { get; set; }
        public bool IsPostLikedByUser { get; set; }
        public string Avatar { get; set; }
        public DateTime Date { get; set; }

        public static Expression<Func<UserPost, UserPostDto>> Selector(int userId)
        {
            return e => new UserPostDto()
            {
                Id = e.Id,
                UserId = e.UserId,
                Content = e.Post.Content,
                ImagePath = e.Post.ImagePath,
                LikesCount = e.Post.Comment.Count,
                PostId = e.PostId,
                IsPostLikedByUser = e.Post.PostLike.Any(d=>d.UserId==userId),
                Avatar = e.User.Avatar,
                Date = e.Post.Date
            };
        }
    }
}