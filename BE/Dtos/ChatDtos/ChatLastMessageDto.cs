using System;

namespace BE.Dtos.ChatDtos
{
    public class ChatLastMessageDto
    {
        public int ChatUrlPart { get; set; }
        public string Content { get; set; }
        public bool HasImage { get; set; }
        public byte[] UserAvatar { get; set; }
        public DateTime Time { get; set; }
    }
}