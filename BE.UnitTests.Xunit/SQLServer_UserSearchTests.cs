using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Search;
using BE.Features.Search.Dtos;
using BE.Features.Search.Services;
using BE.Features.User.Dtos;
using BE.Mapping.Profiles;
using BE.Models;
using BE.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BE.UnitTests
{
    public class SQLServer_UserSearchTests
    {
        private int _userId = 7;
        private IMapper _mapper;

        public SQLServer_UserSearchTests()
        {
            var serviceCollection = new ServiceCollection();
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FoundUserDtoProfile());
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task SQLServer_User_Search_Test()
        {
            var data = GetFakeUsers(20);
            var service = GetService(data);
            var controller = new SQLServer_UserSearchController(service);
            var criteriaDto = new UsersLookUpCriteriaDto
            {
                Name = "Test1",
                Surname = "Test1",
                Interests = new List<UserInterestDto>(){ new UserInterestDto(){Id = 1} }
            };
            var res = await controller.GetUsersByCriteria(1, criteriaDto, 7);
            var obj = res as OkObjectResult;
            Assert.NotNull(obj);
            var actData = obj.Value as List<UserDto>;
            Assert.NotNull(actData);
            Assert.True(actData.Count == 1);
        }
        
        private ISQLServer_UserSearchService GetService(IEnumerable<User> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<User>()).Returns(mockSet.Object);
            var service = new SQLServer_UserSearchService(mockContext.Object, _mapper);
            return service;
        }
        
        private IEnumerable<User> GetFakeUsers(int length)
        {
            for (int i = 1; i <= length; i++)
            {
                yield return new User
                {
                    Id = i,
                    Name = "Test"+i,
                    Surname = "Test"+i,
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
                        new UserInterests{ InterestId = 1, UserId = i }
                    }
                };
            }
        }
    }
}