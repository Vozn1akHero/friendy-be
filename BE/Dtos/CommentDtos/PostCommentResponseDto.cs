using System;
using System.Collections.Generic;
using BE.Dtos.CommentDtos;

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
    }
}