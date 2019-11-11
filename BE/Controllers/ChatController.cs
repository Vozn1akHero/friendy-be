using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IAvatarConverterService _userAvatarConverterService;
        public ChatController(IRepositoryWrapper repository, IAvatarConverterService userAvatarConverterService)
        {
            _repository = repository;
            _userAvatarConverterService = userAvatarConverterService;
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
        public async Task<IActionResult> GetLastMessages([FromHeader(Name = "userId")] int userId)
        {
            var lastMessageList = new List<ChatLastMessageDto>();
            var chatList = await _repository.ChatParticipants.GetUserChatList(userId);
            var chatIdList = chatList.Select(e => e.ChatId).ToList();
            var messageList = await _repository.ChatMessages.GetLastChatMessages(chatIdList);

            foreach (var chatMessage in messageList)
            {
                string chatUrl = await _repository.Chat.GetChatUrlPartById(chatMessage.ChatId);
                lastMessageList.Add(new ChatLastMessageDto
                {
                    ChatUrlPart = chatUrl,
                    Content = chatMessage.Message.Content,
                    HasImage = chatMessage.Message.ImageUrl != null,
                    UserAvatar = _userAvatarConverterService.ConvertToByte(chatMessage.Message.User.Avatar),
                    UserId = chatMessage.Message.User.Id,
                    Date = chatMessage.Message.Date
                });
            }
            
            return Ok(lastMessageList);
        }
        
        [HttpGet]
        [Authorize]
        [Route("participants/friend-basic-data/{chatHash}")]
        public async Task<IActionResult> GetFriendBasicDataInDialog(string chatHash, [FromHeader(Name = "userId")] int userId)
        {
            var chatId = await _repository.Chat.GetChatIdByUrlHash(chatHash);
            var participantsData = await _repository
                .ChatParticipants
                .GetFriendBasicDataInDialogByChatId(chatId, userId);
            return Ok(participantsData);
        }
        
        [HttpGet]
        [Authorize]
        [Route("participants/basic-data/{chatHash}")]
        public async Task<IActionResult> GetDialogParticipantsBasicData(string chatHash)
        {
            var chatId = await _repository.Chat.GetChatIdByUrlHash(chatHash);
            var participantsData = await _repository.ChatParticipants.GetParticipantsBasicDataByChatId(chatId);
            return Ok(participantsData);
        }
        
        [HttpGet]
        [Authorize]
        [Route("messages/{hashUrl}")]
        public async Task<IActionResult> GetMessagesInDialog(string hashUrl, [FromHeader(Name = "userId")] int userId)
        {
            var chatId = await _repository.Chat.GetChatIdByUrlHash(hashUrl);
            var messages = await _repository.ChatMessages.GetByChatId(chatId, userId);
            return Ok(messages);
        }

        [HttpPost]
        [Authorize]
        [Route("message/{chatHash}")]
        public async Task<IActionResult> AddNewMessage(string chatHash, 
            [FromBody] NewMessageDto chatMessage,
            [FromHeader(Name = "userId")] int userId)
        {
            var newMessage = new ChatMessage
            {
                Content = chatMessage.Content,
                ImageUrl = null,
                Date = DateTime.Now,
                UserId = userId
            };
            var chatId = await _repository.Chat.GetChatIdByUrlHash(chatHash);
            await _repository.ChatMessage.Add(newMessage);
            await _repository.ChatMessages.Add(chatId, newMessage.Id);
            
            return CreatedAtAction("AddNewMessage", chatMessage);
        }
        
    }
}