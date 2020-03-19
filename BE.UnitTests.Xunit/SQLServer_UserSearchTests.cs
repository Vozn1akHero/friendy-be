using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.FakeData;
using BE.Features.Search;
using BE.Features.Search.Dtos;
using BE.Features.Search.Services;
using BE.Features.User.Dtos;
using BE.Mapping.Profiles;
using BE.Models;
using BE.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BE.UnitTests.Xunit
{
    public class SQLServer_UserSearchTests
    {
        public SQLServer_UserSearchTests()
        {
            //var serviceCollection = new ServiceCollection();
            var config = new MapperConfiguration(mc => { mc.AddProfile(new FoundUserDtoProfile()); });
            _mapper = config.CreateMapper();
        }

        private int _userId = 7;
        private readonly IMapper _mapper;

        private ISQLServer_UserSearchService GetService(IEnumerable<User> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Xunit.Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<User>()).Returns(mockSet.Object);
            var service = new SQLServer_UserSearchService(mockContext.Object, _mapper);
            return service;
        }

        [Fact]
        public async Task SQLServer_User_Search_Test()
        {
            var data = FakeUserData.GetList(20);
            var service = GetService(data);
            var controller = new SQLServer_UserSearchController(service);
            var criteriaDto = new UsersLookUpCriteriaDto
            {
                Name = "Test1",
                Surname = "Test1",
                Interests = new List<UserInterestDto> {new UserInterestDto {Id = 1}}
            };
            var res = await controller.GetUsersByCriteria(1, criteriaDto, 7);
            var obj = res as OkObjectResult;
            Assert.NotNull(obj);
            var actData = obj.Value as List<UserDto>;
            Assert.NotNull(actData);
            Assert.True(actData.Count == 1);
        }
    }
}