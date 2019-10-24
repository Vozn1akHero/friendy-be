using System;

namespace BE.Dtos.ChatDtos
{
    public class ChatLastMessageDto
    {
        public string ChatUrlPart { get; set; }
        public string Content { get; set; }
        public bool HasImage { get; set; }
        public byte[] UserAvatar { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}