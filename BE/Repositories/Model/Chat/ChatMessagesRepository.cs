using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessages>, IChatMessagesRepository
    {
        private IAvatarConverterService _userAvatarConverterService;

        public ChatMessagesRepository(FriendyContext friendyContext,
            IAvatarConverterService userAvatarConverterService) : base(friendyContext)
        {
            _userAvatarConverterService = userAvatarConverterService;
        }

        public async Task Add(int chatId, int messageId)
        {
            Create(new ChatMessages
            {
                ChatId = chatId,
                MessageId = messageId
            });
            await SaveAsync();
        }

        public async Task<List<ChatMessages>> GetLastChatMessages(List<int> chatIdList)
        {
            var chatMessages = new List<ChatMessages>();
            foreach (var id in chatIdList)
            {
                var messages = await FindByCondition(e => e.ChatId == id)
                    .Include(e => e.Message)
                    .Include(e => e.Message.User)
                    .OrderByDescending(e => e.Message.Date)
                    .FirstOrDefaultAsync();
                chatMessages.Add(messages);
            }
            return chatMessages;
        }

        public async Task<List<ChatMessageDto>> GetByChatId(int chatId, int userId)
        {
            var chatMessages = await FindByCondition(e => e.ChatId == chatId)
                .Include(e => e.Message)
                .Select(e => new ChatMessageDto
                {
                    Content = e.Message.Content,
                    IsUserAuthor = e.Message.UserId == userId,
                    Date = e.Message.Date
                }).ToListAsync();
            
            return chatMessages;
        }
    }
}