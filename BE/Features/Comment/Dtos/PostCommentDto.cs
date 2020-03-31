using System;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Comment.Dtos
{
    public class PostCommentDto
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string AuthorAvatarPath { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int PostId { get; set; }
        public bool IsCommentLikedByUser { get; set; }
        public DateTime Date { get; set; }

        public static Expression<Func<MainComment, PostCommentDto>> Selector(int issuerId)
        {
            return e => new PostCommentDto
            {
                Id = e.Id,
                CommentId = e.Comment.Id,
                AuthorId = e.Comment.User.Id,
                AuthorName = e.Comment.User.Name,
                AuthorSurname = e.Comment.User.Surname,
                AuthorAvatarPath = e.Comment.User.Avatar,
                Content = e.Comment.Content,
                LikesCount = e.Comment.CommentLike.Count,
                CommentsCount = e.ResponseToComment.Count,
                PostId = e.Comment.Post.Id,
                IsCommentLikedByUser =
                    e.Comment.CommentLike.Any(d => d.User.Id == issuerId),
                Date = e.Comment.Date
            };
        }
    }
}