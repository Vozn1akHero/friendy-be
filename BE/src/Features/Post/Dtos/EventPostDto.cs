using System;

namespace BE.Dtos.PostDtos
{
    public class EventPostDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string AvatarPath { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int PostId { get; set; }
        public bool IsPostLikedByUser { get; set; }
        public DateTime Date { get; set; }
    }
}