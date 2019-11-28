using System;

namespace BE.Dtos.ChatDtos
{
    public class ChatLastMessageDto
    {
        public int ChatId { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public int SenderId { get; set; }
        public string SenderAvatarPath { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverAvatarPath { get; set; }
        public int InterlocutorId { get; set; }
        public string InterlocutorAvatarPath { get; set; }
        public bool WrittenByRequestIssuer { get; set; }
    }
}