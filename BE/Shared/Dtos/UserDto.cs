using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BE.Features.Search.Dtos;
using BE.Models;

namespace BE.Shared.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public int? EducationId { get; set; }
        public bool OnlineStatus { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? AlcoholAttitudeId { get; set; }
        public int? SmokingAttitudeId { get; set; }
        //public bool AreFriends { get; set; }
        public FriendshipStatus FriendshipStatus { get; set; }
        public IEnumerable<object> UserInterests { get; set; }

        private static FriendshipStatus GetFriendshipStatus(int issuerId,
            int userId,
            IEnumerable<UserFriendship> friendshipFF,
            IEnumerable<UserFriendship> friendshipSF,
            IEnumerable<FriendRequest> friendRequestReceiver, 
            IEnumerable<FriendRequest> friendRequestAuthor)
        {
            if (issuerId == userId) return FriendshipStatus.ISSUER_ACC;
            bool areFriends = friendshipFF.Any(d => d.FirstFriendId == issuerId &&
                                                                   d.SecondFriendId == userId
                                                                   || d.SecondFriendId == issuerId &&
                                                                   d.FirstFriendId == userId) || friendshipSF.Any(d =>
                                      d.FirstFriendId == issuerId && d.SecondFriendId == userId
                                      || d.SecondFriendId == issuerId && d.FirstFriendId == userId);
            if (areFriends) return FriendshipStatus.FRIEND;
            bool isRequestSentByIssuer = friendRequestReceiver.Any(e => e.AuthorId == issuerId);
            if (isRequestSentByIssuer) return FriendshipStatus.REQUEST_SENT;
            bool isRequestReceivedByIssuer = friendRequestAuthor.Any(e => e.ReceiverId == issuerId);
            if (isRequestReceivedByIssuer) return FriendshipStatus.REQUEST_RECEIVED;
            return FriendshipStatus.NOT_FRIEND;
        }
        
        public static Expression<Func<Models.User, UserDto>> Selector(int issuerId)
        {
            return e => new UserDto
            {
                Id = e.Id,
                Name = e.Name,
                Surname = e.Surname,
                GenderId = e.GenderId,
                Birthday = e.Birthday,
                Avatar = e.Avatar,
                City = e.City.Title,
                EducationId = e.EducationId,
                OnlineStatus = e.Session.ConnectionEnd != null,
                MaritalStatusId = e.AdditionalInfo.MaritalStatusId,
                ReligionId = e.AdditionalInfo.ReligionId,
                AlcoholAttitudeId = e.AdditionalInfo.AlcoholAttitudeId,
                SmokingAttitudeId = e.AdditionalInfo.SmokingAttitudeId,
                FriendshipStatus = GetFriendshipStatus(issuerId, e.Id, e.UserFriendshipFirstFriend, e
                .UserFriendshipSecondFriend,  e.FriendRequestReceiver, e.FriendRequestAuthor),
                UserInterests = e.UserInterests.Select(d => new UserInterestDto
                {
                    Id = d.Interest.Id,
                    Title = d.Interest.Title
                })
            };
        }
    }
}