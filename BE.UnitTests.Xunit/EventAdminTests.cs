using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BE.Features.Event;
using BE.Features.Event.Services;
using BE.Models;
using BE.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BE.UnitTests
{
    public class EventAdminTests
    {
        private int _userId = 7;
        private int _eventId = 1;

        public EventAdminTests()
        {
            var serviceCollection = new ServiceCollection();
            
        }

        [Fact]
        public void GetRange()
        {
            var data = GetFakeData(20);
            var controller = new EventAdminsController(GetRepository(data),
                GetService(data));
            var res = controller.GetAdminList(1, 1, 20, _userId);    
            Assert.NotNull(res);
        }
        
        private IEventAdminService GetService(IEnumerable<EventAdmins> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<EventAdmins>()).Returns(mockSet.Object);
            var repositoryMock = new Mock<RepositoryWrapper>(mockContext.Object);
            var service = new EventAdminService(repositoryMock.Object);
            return service;
        }

        private IRepositoryWrapper GetRepository(IEnumerable<EventAdmins> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<EventAdmins>()).Returns(mockSet.Object);
            var repository = new RepositoryWrapper(mockContext.Object);
            return repository;
        }

        private IEnumerable<EventAdmins> GetFakeData(int i)
        {
            for (int j = 1; j < i; j++)
            {
                yield return new EventAdmins()
                {
                    Event = new Event
                    {
                        Id = _eventId
                    },
                    User = new User
                    {
                        Id = j
                    }
                };
            }
        }
    }
}