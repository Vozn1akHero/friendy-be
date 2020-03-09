using System;
using System.Threading.Tasks;
using BE.Features.Chat.Dtos;
using BE.Features.Chat.Helpers;
using BE.Features.Chat.Services;
using BE.SignalR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Chat
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IDialogNotifier _dialogNotifier;

        public ChatController(IDialogNotifier dialogNotifier,
            IChatService chatService)
        {
            _dialogNotifier = dialogNotifier;
            _chatService = chatService;
        }
        
        [HttpGet]
        [Authorize]
        [Route("last-messages/paginate")]
        public IActionResult GetLastMessagesWithPagination([FromQuery(Name =
                "page")]
            int page, [FromHeader(Name = "userId")] int userId)
        {
            var lastMessageList = _chatService
                .GetLastMessageByReceiverIdWithPagination(userId, page);
            return Ok(lastMessageList);
        }

        [HttpGet]
        [Authorize]
        [Route("data-by-interlocutors/{to}")]
        public async Task<IActionResult> GetByInterlocutorsIdentifiers(int to,
            [FromHeader(Name = "userId")] int userId)
        {
            var res = await _chatService.GetByInterlocutorsIdentifiers(to, userId);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        [Route("{to}/page/{page}")]
        public IActionResult GetMessageInDialogWithPaginationAsync(int to, int page,
            [FromHeader(Name = "userId")] int userId)
        {
            var res = _chatService
                .GetMessageByReceiverIdWithPagination(to, userId, page);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        [Route("message/{chatId}/{receiverId}")]
        public async Task<IActionResult> AddNewMessage(int receiverId,
            int chatId,
            [FromForm(Name = "text")] string text,
            [FromForm(Name = "image")] IFormFile file,
            [FromHeader(Name = "userId")] int userId)
        {
            try
            {
                var newMessage = await _chatService.CreateAndReturnMessageAsync(text, file, chatId,
                    userId, receiverId);
                await _dialogNotifier.SendNewMessageAsync(Convert.ToString(chatId), new
                    CreatedMessageDto
                    {
                        Content = newMessage.Content,
                        Date = newMessage.Date,
                        ImagePath = newMessage.ImagePath,
                        UserId = newMessage.UserId
                    });
                var obj = _chatService.GetLastChatMessageByChatId(chatId, receiverId);
                await _dialogNotifier.SendNewExpandedMessageAsync(Convert.ToString
                    (chatId), obj);
                return CreatedAtAction("AddNewMessage", newMessage);
            }
            catch (EmptyMessageException)
            {
                return UnprocessableEntity("MESSAGE CANNOT BE EMPTY");
            }
        }
    }
}