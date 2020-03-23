using System;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Friendship.Dtos
{
    public class ReceivedFriendRequestDto
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }
        public int RequestId { get; set; }
        public bool IsOnline { get; set; }

        public static Expression<Func<FriendRequest, ReceivedFriendRequestDto>> Selector =>
            e => new ReceivedFriendRequestDto()
            {
                AuthorId = e.Author.Id,
                Name = e.Author.Name,
                Surname = e.Author.Surname,
                Avatar = e.Author.Avatar,
                RequestId = e.Id,
                IsOnline = e.Author.Session.ConnectionEnd != null
            };
    }
}