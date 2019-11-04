using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BE.Dtos.ChatDtos;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessages>, IChatMessagesRepository
    {
        private readonly IMapper _mapper;
        public ChatMessagesRepository(FriendyContext friendyContext,
            IMapper mapper) : base(friendyContext)
        {
            _mapper = mapper;
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
/*            var chatMessages = FindByCondition(e => e.ChatId == chatId)
                .ProjectTo<ChatMessageDto>(_mapper).ToListAsync();*/
                
            var chatMessages = await FindByCondition(e => e.ChatId == chatId)
                .Include(e => e.Message)
                .Select(e => new ChatMessageDto()
                {
                    Content = e.Message.Content,
                    IsUserAuthor = e.Message.UserId == userId,
                    Date = e.Message.Date
                }).ToListAsync();
            
            return chatMessages;
        }
    }
}