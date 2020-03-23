using System;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Friendship.Dtos
{
    public class SentFriendRequestDto
    {
        public int ReceiverId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }
        public int RequestId { get; set; }
        public bool IsOnline { get; set; }

        public static Expression<Func<FriendRequest, SentFriendRequestDto>> Selector =>
            e => new SentFriendRequestDto()
            {
                ReceiverId = e.ReceiverId,
                Name = e.Receiver.Name,
                Surname = e.Receiver.Surname,
                Avatar = e.Receiver.Avatar,
                RequestId = e.Id,
                IsOnline = e.Receiver.Session.ConnectionEnd != null
            };
    }
}