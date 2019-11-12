using System;

namespace BE.Dtos
{
    public class PostOnWallDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsPostLikedByUser { get; set; }
        public DateTime Date { get; set; }
    }
}