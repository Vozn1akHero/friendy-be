using System;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Models;
using BE.Services.Global;
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
        private IDialogNotifierService _dialogNotifierService;
        
        public ChatController(IRepositoryWrapper repository,
            IImageSaver imageSaver, 
            IDialogNotifierService dialogNotifierService)
        {
            _repository = repository;
            _imageSaver = imageSaver;
            _dialogNotifierService = dialogNotifierService;
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
        [Route("last-messages")]
        public async Task<IActionResult> GetLastMessages([FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var lastMessageList = await _repository
                .ChatMessages
                .GetLastChatMessageRangeByReceiverId(userId, startIndex, length);
            return Ok(lastMessageList);
        }


/*        [HttpGet]
        [Authorize]
        [Route("participants/basic-data/{chatHash}")]
        public async Task<IActionResult> GetDialogParticipantsBasicData(string chatHash)
        {
            var chatId = await _repository.Chat.GetChatIdByUrlHash(chatHash);
            var participantsData = await _repository.ChatParticipants.GetParticipantsBasicDataByChatId(chatId);
            return Ok(participantsData);
        }*/

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
        [Route("{to}")]
        public async Task<IActionResult> GetMessageRangeInDialogAsync(int to,
            [FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            //var chatId = await _repository.Chat.GetChatIdByUrlHash(hashUrl);
            var messages = await _repository.ChatMessages.GetMessageRangeByReceiverId(to, userId, startIndex, length);
            return Ok(messages);
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
            
            await _dialogNotifierService.SendNewMessageAsync(Convert.ToString(chatId), new 
            CreatedMessageDto
            {
                Content = newMessage.Content,
                Date = newMessage.Date,
                ImagePath = newMessage.ImagePath,
                UserId = newMessage.UserId
            });
            
            var obj = await _repository.Chat.GetLastChatMessageByChatId(chatId);
            await _dialogNotifierService.SendNewExpandedMessageAsync(Convert.ToString(receiverId), obj);
            
            return CreatedAtAction("AddNewMessage", newMessage);
        }
    }
}