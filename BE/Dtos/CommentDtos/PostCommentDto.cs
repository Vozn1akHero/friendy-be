using System;

namespace BE.Dtos.CommentDtos
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
    }
}