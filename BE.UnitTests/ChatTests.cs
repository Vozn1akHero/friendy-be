using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.FakeData;
using BE.Features.Chat;
using BE.Features.Chat.Dtos;
using BE.Features.Chat.Repositories;
using BE.Features.Chat.Services;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using BE.SignalR.Hubs;
using BE.SignalR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BE.UnitTests
{
    public class ChatTests
    {
        private IImageSaver _imageSaver;
        private IDialogNotifier _dialogNotifier;

        public ChatTests()
        {
            var mockClientProxy = new Mock<IClientProxy>();
            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            var dialogHubMock = new Mock<IHubContext<DialogHub>>();
            dialogHubMock.Setup(x => x.Clients).Returns(() => mockClients.Object);
            

            var provider = new ServiceCollection()
                .AddSingleton<IRepositoryWrapper, RepositoryWrapper>()
                .AddSingleton<IImageSaver, ImageSaver>()
                .AddSingleton<IDialogNotifier, DialogNotifier>()
                .BuildServiceProvider();
            _imageSaver = provider.GetService<ImageSaver>();
            
        }

        [Test]
        public void Get_Dialogs_Test()
        {
            var chat1 = Enumerable.Range(1, 20)
                .Select(e => FakeChatMessages.Create(e, 1, e, 1, 2));
            var chat2 = Enumerable.Range(21, 10)
                .Select(e => FakeChatMessages.Create(e, 2, e, 1, 3));
            var chat3 = Enumerable.Range(31, 10)
                .Select(e => FakeChatMessages.Create(e, 3, e, 2, 3));
            var messages = new List<ChatMessages>(chat1
                .Concat(chat2)
                .Concat(chat3));
            var controller = GetController(messages);
            var res = controller.GetDialogList(1, 3, 1);
            Assert.NotNull(res);
            var value = (res as OkObjectResult).Value as List<ChatLastMessageDto>;
            Assert.NotNull(value);
            Assert.AreEqual(2, value.Count);
        }

        [Test]
        public async Task Get_Interlocutors_Data_Test()
        {
            var chatMessages = Enumerable.Range(1, 20)
                .Select(e => FakeChatMessages.Create(e, 1, e, 1, 2));
            var controller = GetController(chatMessages);
            var res = await controller.GetByInterlocutorsIdentifiersAsync(2, 1);
            Assert.NotNull(res);
            var value = (res as OkObjectResult).Value as InterlocutorsDto;
            Assert.NotNull(value);
        }

        [Test]
        public void Get_Message_In_Dialog_With_Pagination_Test()
        {
            var chat1 = Enumerable.Range(1, 20)
                .Select(e => FakeChatMessages.Create(e, 1, e, 1, 2));
            var chat2 = Enumerable.Range(21, 10)
                .Select(e => FakeChatMessages.Create(e, 2, e, 1, 3));
            var chat3 = Enumerable.Range(31, 10)
                .Select(e => FakeChatMessages.Create(e, 3, e, 2, 3));
            var messages = new List<ChatMessages>(chat1
                .Concat(chat2)
                .Concat(chat3));
            var controller = GetController(messages);
            var res = controller.GetMessageInDialogWithPaginationAsync(2, 1, 5, 1);
            Assert.NotNull(res);
            var value = (res as OkObjectResult).Value as List<ChatMessageDto>;
            Assert.NotNull(value);
            Assert.AreEqual(5, value.Count);
        }

        [Test]
        public async Task Add_New_Message_Test()
        {
            var chat1 = Enumerable.Range(1, 20)
                .Select(e => FakeChatMessages.Create(e, 1, e, 1, 2));
            var controller = GetController(chat1);
            var res = await controller.AddNewMessage(2, 1, "test", null, 1);
            Assert.NotNull(res);
            var value = (res as CreatedAtActionResult).Value as ChatMessage;
            Assert.NotNull(value);
        }

        private ChatController GetController(IEnumerable<ChatMessages> data)
        {
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<ChatMessages>()).Returns(mockSet.Object);
            var mockWrapper = new Mock<RepositoryWrapper>(mockContext.Object);
            var rep = new ChatMessagesRepository(mockContext.Object);
            mockWrapper.Setup(c => c.ChatMessages).Returns(rep);
            var service = new ChatService(mockWrapper.Object, _imageSaver);
            var controller = new ChatController(_dialogNotifier, service);
            return controller;
        }
    }
}