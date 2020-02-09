using System;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Models;
using BE.Services.Global;
using BE.Services.Model;
using BE.SignalR.Hubs;
using BE.SignalR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IImageSaver _imageSaver;
        private readonly IRepositoryWrapper _repository;
        private readonly IDialogNotifier _dialogNotifier;
        private readonly IChatService _chatService;
        
        public ChatController(IRepositoryWrapper repository,
            IImageSaver imageSaver, 
            IDialogNotifier dialogNotifier, IChatService chatService)
        {
            _repository = repository;
            _imageSaver = imageSaver;
            _dialogNotifier = dialogNotifier;
            _chatService = chatService;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }


        [HttpGet]
        [Authorize]
        [Route("last-messages/paginate")]
        public async Task<IActionResult> GetLastMessagesWithPagination([FromQuery(Name = 
        "page")] int page, [FromHeader(Name = "userId")] int userId)
        {
            var lastMessageList = await _chatService
                .GetLastMessageByReceiverIdWithPagination(userId, page);
            return Ok(lastMessageList);
        }

        [HttpGet]
        [Authorize]
        [Route("data-by-interlocutors/{to}")]
        public async Task<IActionResult> GetByInterlocutorsIdentifiers(int to, [FromHeader(Name = "userId")] int userId)
        {
            var res = await _repository.Chat.GetByInterlocutorsIdentifiers(to, userId);
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        [Route("{to}/page/{page}")]
        public async Task<IActionResult> GetMessageInDialogWithPaginationAsync(int to, int page,
            [FromHeader(Name = "userId")] int userId)
        {
            //var chatId = await _repository.Chat.GetChatIdByUrlHash(hashUrl);
            //var messages = await _repository.ChatMessages.GetMessageRangeByReceiverId(to, userId, startIndex, length);
            var res = await _chatService
                .GetMessageByReceiverIdWithPagination(to, userId, page);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        [Route("message/{chatId}/{receiverId}")]
        public async Task<IActionResult> AddNewMessage(int receiverId,
            int chatId,
            [FromBody] NewMessageDto chatMessage,
            [FromHeader(Name = "userId")] int userId)
        {
            string imagePath = null;
            
            if (chatMessage.File != null)
            {
                imagePath = await _imageSaver.SaveAndReturnImagePath(chatMessage.File, 
                "ChatPhoto", chatId);
                var image = new Image
                {
                    Path = imagePath,
                    PublishDate = DateTime.Now
                };
                await _repository.Photo.Add(image);
            }
            
            var newMessage = new ChatMessage
            {
                Content = chatMessage.Content,
                ImagePath = imagePath,
                UserId = userId,
                Date = DateTime.Now,
                ReceiverId = receiverId
            };
            
            await _repository.ChatMessage.Add(newMessage);

            var chatMessages = new ChatMessages
            {
                ChatId = chatId,
                MessageId = newMessage.Id
            };
            
            await _repository.ChatMessages.Add(chatMessages);
            
            await _dialogNotifier.SendNewMessageAsync(Convert.ToString(chatId), new 
            CreatedMessageDto
            {
                Content = newMessage.Content,
                Date = newMessage.Date,
                ImagePath = newMessage.ImagePath,
                UserId = newMessage.UserId
            });
            
            var obj = await _chatService.GetLastChatMessageByChatId(chatId, receiverId);
            await _dialogNotifier.SendNewExpandedMessageAsync(Convert.ToString
            (receiverId), obj);
            
            return CreatedAtAction("AddNewMessage", newMessage);
        }
    }
}