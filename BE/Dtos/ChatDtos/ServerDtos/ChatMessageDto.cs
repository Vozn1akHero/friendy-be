using System;

namespace BE.Dtos.ChatDtos
{
    public class ChatMessageDto
    {
        public string Content { get; set; }
        public bool IsUserAuthor { get; set; }
        public DateTime Date { get; set; }
    }
}