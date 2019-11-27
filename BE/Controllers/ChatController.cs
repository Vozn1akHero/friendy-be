using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IAvatarConverterService _userAvatarConverterService;
        
        public ChatController(IRepositoryWrapper repository,
            IAvatarConverterService userAvatarConverterService)
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
            return Ok(await _repository.Chat.GetByInterlocutorsIdentifiers(to, userId));
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
        [Route("message/{receiverId}")]
        public async Task<IActionResult> AddNewMessage(int receiverId, 
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
/*            var chatId = await _repository.Chat.GetChatIdByUrlHash(chatHash);
            await _repository.ChatMessage.Add(newMessage);
            await _repository.ChatMessages.Add(chatId, newMessage.Id);*/
            
            return CreatedAtAction("AddNewMessage", chatMessage);
        }
    }
}