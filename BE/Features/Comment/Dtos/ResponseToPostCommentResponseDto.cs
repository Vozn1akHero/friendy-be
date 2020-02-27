using System;

namespace BE.Features.Comment.Dtos
{
    public class ResponseToPostCommentResponseDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string AuthorAvatarPath { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public string CommentAuthorName { get; set; }
        public string CommentAuthorSurname { get; set; }
        public bool IsCommentLikedByUser { get; set; }
        public DateTime Date { get; set; }
    }
}