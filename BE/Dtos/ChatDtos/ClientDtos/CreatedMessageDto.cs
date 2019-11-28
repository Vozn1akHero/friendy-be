using System;

namespace BE.Dtos.ChatDtos
{
    public class CreatedMessageDto
    {
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}