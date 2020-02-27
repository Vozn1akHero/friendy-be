using System;
using System.Linq.Expressions;
using BE.Features.Friendship.Dtos;
using BE.Models;

namespace BE.Features.Chat.Dtos
{
    public class ChatLastMessageDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        //public int SenderId { get; set; }
        //public string SenderAvatarPath { get; set; }
        public FriendDto Sender { get; set; }

        /*public int ReceiverId { get; set; }
        public string ReceiverAvatarPath { get; set; }*/
        public FriendDto Receiver { get; set; }

/*        public int InterlocutorId { get; set; }
        public string InterlocutorAvatarPath { get; set; }*/
        public FriendDto Interlocutor { get; set; }
        public bool WrittenByRequestIssuer { get; set; }

        public static Expression<Func<ChatMessages, ChatLastMessageDto>> Selector(
            int receiverId)
        {
            return e => new ChatLastMessageDto
            {
                Id = e.Id,
                ChatId = e.ChatId,
                Content = e.Message.Content,
                ImageUrl = e.Message.ImagePath,
                Date = e.Message.Date,
                /*SenderId = e.Message.UserId,
                SenderAvatarPath = e.Message.User.Avatar,*/
                Sender = new FriendDto
                {
                    Id = e.Message.UserId,
                    Name = e.Message.User.Name,
                    Surname = e.Message.User.Surname,
                    OnlineStatus = e.Message.User.Session.ConnectionEnd != null,
                    AvatarPath = e.Message.User.Avatar
                },
                /*ReceiverId = e.Message.ReceiverId,
                ReceiverAvatarPath = e.Message.Receiver.Avatar,*/
                Receiver = new FriendDto
                {
                    Id = e.Message.ReceiverId,
                    Name = e.Message.Receiver.Name,
                    Surname = e.Message.Receiver.Surname,
                    OnlineStatus = e.Message.Receiver.Session.ConnectionEnd != null,
                    AvatarPath = e.Message.Receiver.Avatar
                },
                Interlocutor = receiverId == e.Message.ReceiverId
                    ? new FriendDto
                    {
                        Id = e.Message.UserId,
                        Name = e.Message.User.Name,
                        Surname = e.Message.User.Surname,
                        OnlineStatus = e.Message.User.Session.ConnectionEnd != null,
                        AvatarPath = e.Message.User.Avatar
                    }
                    : new FriendDto
                    {
                        Id = e.Message.Receiver.Id,
                        Name = e.Message.Receiver.Name,
                        Surname = e.Message.Receiver.Surname,
                        OnlineStatus = e.Message.Receiver.Session != null,
                        AvatarPath = e.Message.Receiver.Avatar
                    },
                WrittenByRequestIssuer = receiverId == e.Message.UserId
            };
        }
    }
}