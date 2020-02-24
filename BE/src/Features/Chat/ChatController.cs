using System;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos.ClientDtos;
using BE.Features.Chat.Services;
using BE.Repositories;
using BE.SignalR.Services;
using Microsoft.AspNetCore.Authorization;
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
            [FromBody] NewMessageDto chatMessage,
            [FromHeader(Name = "userId")] int userId)
        {
            var newMessage = await _chatService.CreateAndReturnMessageAsync(chatMessage,
                chatId,
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
                (receiverId), obj);
            return CreatedAtAction("AddNewMessage", newMessage);
        }
    }
}