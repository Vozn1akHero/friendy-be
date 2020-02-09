using System;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Dtos.ChatDtos
{
    public class ChatLastMessageDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
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
        
        public static Expression<Func<ChatMessages, ChatLastMessageDto>> Selector(int receiverId)
        {
            return e => new ChatLastMessageDto
                {
                    Id = e.Id,
                    ChatId = e.ChatId,
                    Content = e.Message.Content,
                    ImageUrl = e.Message.ImagePath,
                    Date = e.Message.Date,
                    SenderId = e.Message.UserId,
                    SenderAvatarPath = e.Message.User.Avatar,
                    ReceiverId = e.Message.ReceiverId,
                    ReceiverAvatarPath = e.Message.Receiver.Avatar,
                    InterlocutorId = receiverId == e.Message.ReceiverId ? e.Message
                    .UserId : e.Message.ReceiverId,
                    InterlocutorAvatarPath = receiverId == e.Message.ReceiverId ? e.Message
                        .User.Avatar : e.Message.Receiver.Avatar,
                    WrittenByRequestIssuer = receiverId == e.Message.UserId
                };
            }
    }
}