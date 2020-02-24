using System;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Comment.Dto
{
    public class PostCommentResponseDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string AuthorAvatarPath { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int PostId { get; set; }
        public int? CommentId { get; set; }
        public bool IsCommentLikedByUser { get; set; }
        public string CommentAuthorName { get; set; }
        public string CommentAuthorSurname { get; set; }
        public DateTime Date { get; set; }
        public int? ResponseToCommentId { get; set; }

        public static Expression<Func<ResponseToComment, PostCommentResponseDto>> Selector
            (int userId)
        {
            return e => new PostCommentResponseDto
            {
                Id = e.Id,
                AuthorId = e.Comment.UserId,
                AuthorName = e.Comment.User.Name,
                AuthorSurname = e.Comment.User.Surname,
                AuthorAvatarPath = e.Comment.User.Avatar,
                Content = e.Comment.Content,
                LikesCount = e.CommentResponseLike.Count,
                PostId = e.Comment.PostId,
                CommentId = e.MainComment.CommentId,
                CommentAuthorName = e.MainComment.Comment.User.Name,
                CommentAuthorSurname = e.MainComment.Comment.User.Surname,
                IsCommentLikedByUser = e.CommentResponseLike.Any(d => d.UserId == userId),
                Date = e.Comment.Date,
                ResponseToCommentId = e.ResponseToCommentId
            };
        }
    }
}