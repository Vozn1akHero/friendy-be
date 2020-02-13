using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BE.Dtos.CommentDtos;
using BE.Models;

namespace BE.Dtos
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
                CommentId = e.ResponseToCommentNavigation.Id,
                CommentAuthorName = e.ResponseToCommentNavigation.User.Name,
                CommentAuthorSurname = e.ResponseToCommentNavigation.User.Surname,
                IsCommentLikedByUser = e.CommentResponseLike.Any(d => d.UserId == userId),
                Date = e.Comment.Date
            };
        }
    }
}