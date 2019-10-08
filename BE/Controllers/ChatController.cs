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

            messageList.ForEach(value =>
            {
                using (FileStream fs = new FileStream(value.Message.User.Avatar, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(value.Message.User.Avatar);
                    fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    
                    lastMessageList.Add(new ChatLastMessageDto
                    {
                        ChatUrlPart = value.ChatId,
                        Content = value.Message.Content,
                        HasImage = value.Message.ImageUrl != null,
                        UserAvatar = bytes,
                        UserId = value.Message.User.Id,
                        Date = value.Message.Date
                    });
                }
            });
            
            return Ok(lastMessageList);
        }
        
        [HttpGet]
        [Authorize]
        [Route("getChat/{hashUrl}")]
        public async Task<IActionResult> GetChat(string hashUrl)
        {
            return Ok();
        }
    }
}