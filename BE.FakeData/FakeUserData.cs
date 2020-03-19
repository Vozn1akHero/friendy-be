using System;
using System.Collections.Generic;
using BE.Models;

namespace BE.FakeData
{
    public static class FakeUserData
    {
        public static User CreateById(int id)
        {
            return new User()
            {
                Id = id,
                Name = "Test"+id,
                Surname = "Test"+id,
                GenderId = 1,
                Birthday = new DateTime(1999, 4, 10),
                Avatar = "",
                EducationId = 1,
                Session = new Session
                {
                    Id = 1,
                    ConnectionId = Guid.NewGuid().ToString(),
                    ConnectionStart = DateTime.Today,
                    ConnectionEnd = DateTime.Now
                },
                AdditionalInfo = new UserAdditionalInfo
                {
                    AlcoholAttitudeId = 1,
                    MaritalStatusId = 1,
                    ReligionId = 1,
                    SmokingAttitudeId = 1
                },
                UserFriendshipFirstFriend = new List<UserFriendship>(),
                UserFriendshipSecondFriend = new List<UserFriendship>(),
                UserInterests = new List<UserInterests>()
                {
                    new UserInterests{ InterestId = 1, UserId = id }
                }
            };
        }
        
        public static IEnumerable<User> GetList(int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return CreateById(i);
            }
        }
    }
}