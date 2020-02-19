using System;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Comment.Dto
{
    public class PostCommentDto
    {
        public int Id { get; set; }
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
                Id = e.Comment.Id,
                AuthorId = e.Comment.UserId,
                AuthorName = e.Comment.User.Name,
                AuthorSurname = e.Comment.User.Surname,
                AuthorAvatarPath = e.Comment.User.Avatar,
                Content = e.Comment.Content,
                LikesCount = e.Comment.CommentLike.Count,
                CommentsCount = e.ResponseToComment.Count,
                PostId = e.Comment.PostId,
                IsCommentLikedByUser =
                    e.Comment.CommentLike.Any(d => d.UserId == issuerId),
                Date = e.Comment.Date
            };
        }

        /*public static Expression<Func<Comment, PostCommentDto>> DefaultCommentSelector(int issuerId)
        {
            return e=>new PostCommentDto
            {
                Id = e.Id,
                AuthorId = e.UserId,
                AuthorName = e.User.Name,
                AuthorSurname = e.User.Surname,
                AuthorAvatarPath = e.User.Avatar,
                Content = e.Content,
                LikesCount = e.CommentLike.Count,
                CommentsCount = e..Count,
                PostId = e.PostId,
                IsCommentLikedByUser = e.CommentLike.Any(d => d.UserId == issuerId),
                Date = e.Date
            };
        }*/
    }
}