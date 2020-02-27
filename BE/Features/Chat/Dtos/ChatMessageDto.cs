using System;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Chat.Dtos
{
    public class ChatMessageDto
    {
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public static Expression<Func<ChatMessages, ChatMessageDto>> Selector
        {
            get
            {
                return e => new ChatMessageDto
                {
                    Content = e.Message.Content,
                    ImagePath = e.Message.ImagePath,
                    UserId = e.Message.UserId,
                    Date = e.Message.Date
                };
            }
        }
    }
}