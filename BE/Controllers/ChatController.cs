using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ChatController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
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
        [Route("getLastMessages")]
        public async Task<IActionResult> GetLastMessages([FromHeader(Name = "userId")] int userId)
        {
            var lastMessageList = new List<ChatLastMessageDto>();
            var chatList = await _repositoryWrapper.ChatParticipants.GetUserChatList(userId);
            var chatIdList = chatList.Select(e => e.ChatId).ToList();
            var messageList = await _repositoryWrapper.ChatMessages.GetLastChatMessages(chatIdList);

            foreach (var chatMessage in messageList)
            {
                using (FileStream fs = new FileStream(chatMessage.Message.User.Avatar, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(chatMessage.Message.User.Avatar);
                    fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    
                    string chatUrl = await _repositoryWrapper.Chat.GetChatUrlPartById(chatMessage.ChatId);
                    lastMessageList.Add(new ChatLastMessageDto
                    {
                        ChatUrlPart = chatUrl,
                        Content = chatMessage.Message.Content,
                        HasImage = chatMessage.Message.ImageUrl != null,
                        UserAvatar = bytes,
                        UserId = chatMessage.Message.User.Id,
                        Date = chatMessage.Message.Date
                    });
                }
            }
            
            return Ok(lastMessageList);
        }
        
        [HttpGet]
        [Authorize]
        [Route("participants/friend-basic-data/{chatHash}")]
        public async Task<IActionResult> GetFriendBasicDataInDialog(string chatHash, [FromHeader(Name = "userId")] int userId)
        {
            var chatId = await _repositoryWrapper.Chat.GetChatIdByUrlHash(chatHash);
            var participantsData = await _repositoryWrapper
                .ChatParticipants
                .GetFriendBasicDataInDialogByChatId(chatId, userId);
            return Ok(participantsData);
        }
        
        [HttpGet]
        [Authorize]
        [Route("participants/basic-data/{chatHash}")]
        public async Task<IActionResult> GetDialogParticipantsBasicData(string chatHash)
        {
            var chatId = await _repositoryWrapper.Chat.GetChatIdByUrlHash(chatHash);
            var participantsData = await _repositoryWrapper.ChatParticipants.GetParticipantsBasicDataByChatId(chatId);
            return Ok(participantsData);
        }
        
        [HttpGet]
        [Authorize]
        [Route("messages/{hashUrl}")]
        public async Task<IActionResult> GetMessagesInDialog(string hashUrl, [FromHeader(Name = "userId")] int userId)
        {
            var chatId = await _repositoryWrapper.Chat.GetChatIdByUrlHash(hashUrl);
            var messages = await _repositoryWrapper.ChatMessages.GetByChatId(chatId, userId);
            return Ok(messages);
        }
    }
}