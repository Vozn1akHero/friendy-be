using System;

namespace BE.Dtos.PostDtos
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
    }
}