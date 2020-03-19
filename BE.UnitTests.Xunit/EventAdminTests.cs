using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.FakeData;
using BE.Features.Event;
using BE.Features.Event.Dtos;
using BE.Features.Event.Repositories;
using BE.Features.Event.Services;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BE.UnitTests.Xunit
{
    public class EventAdminTests
    {
        private readonly int _userId = 7;
        private int _eventId = 1;

        private IEventAdminService GetService(IEnumerable<EventAdmins> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<EventAdmins>()).Returns(mockSet.Object);
            var repositoryMock = new Mock<RepositoryWrapper>(mockContext.Object);
            var rep = new EventAdminsRepository(mockContext.Object);
            repositoryMock.Setup(c => c.EventAdmins).Returns(rep);
            var service = new EventAdminService(repositoryMock.Object);
            return service;
        }

        private IRepositoryWrapper GetRepository(IEnumerable<EventAdmins> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<EventAdmins>()).Returns(mockSet.Object);
            /*var mockWrapper = new Mock<RepositoryWrapper>(mockContext.Object);
            var rep = new EventAdminsRepository(mockContext.Object);
            mockWrapper.Setup(c => c.EventAdmins).Returns(rep);
            return mockWrapper.Object;*/
            return new RepositoryWrapper(mockContext.Object);
        }

        private IEnumerable<EventAdmins> GetFakeData(int i)
        {
            for (var j = 1; j < i; j++)
                yield return new EventAdmins
                {
                    Id = j,
                    EventId = _eventId,
                    UserId = j,
                    Event = FakeEventData.CreateById(_eventId, 
                        _userId),
                    User = FakeUserData.CreateById(j)
                };
        }
        
        private EventAdminsController GetController(){
            var data = GetFakeData(20).ToList();
            var repository = GetRepository(data);
            var service = GetService(data);
            return new EventAdminsController(repository, service);
        }

        [Fact]
        public void GetRange()
        {
            var controller = GetController();
            var res = (controller.GetAdminList(1, 1, 20)
                    as OkObjectResult).Value
                as List<EventAdminDto>;
            Assert.NotNull(res);
            Assert.Equal(1, res.ElementAt(0).Id);
        }

        [Fact]
        public async Task Throw422OnCreation()
        {
            var controller = GetController();
            var resUnpEnt = await controller.CreateAsync(_eventId, _userId);
            Assert.NotNull(resUnpEnt);
            Assert.IsAssignableFrom<UnprocessableEntityObjectResult>(resUnpEnt);
        }
        
        [Fact]
        public async Task Create()
        {
            int fakeUserId = new Random().Next(2000, 3000);
            var controller = GetController();
            var res = await controller.CreateAsync(_eventId, fakeUserId);
            var val = (res as CreatedAtActionResult).Value as EventAdmins;
            Assert.NotNull(res);
            Assert.Equal(_eventId, val.EventId);
            Assert.Equal(fakeUserId, val.UserId);
        }
    }
}