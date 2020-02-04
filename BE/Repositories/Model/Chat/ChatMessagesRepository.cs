using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessages>, IChatMessagesRepository
    {
        public ChatMessagesRepository(FriendyContext friendyContext) : base(friendyContext)
        {}

        public async Task Add(ChatMessages chatMessages)
        {
            Create(chatMessages);
            await SaveAsync();
        }

        
        public async Task<IEnumerable<ChatMessages>> GetMessageRangeByReceiverId(int 
        receiverId, int 
        issuerId, int startIndex, int length)
        {
            var chatMessages = await FindByCondition(e =>
                    (e.Chat.FirstParticipantId == receiverId
                     && e.Chat.SecondParticipantId == issuerId)
                    || (e.Chat.FirstParticipantId == issuerId
                        && e.Chat.SecondParticipantId == receiverId)
                    && e.MessageId >= startIndex)
                .Include(e => e.Message)
                .Take(length)
                .ToListAsync();
            return chatMessages;
        }

        public async Task<IEnumerable<ChatMessages>> GetMessageByReceiverIdWithPaginationAsync(int receiverId, int issuerId, int page)
        {
            int length = 20;
            var chatMessages = await FindByCondition(e =>
                    (e.Chat.FirstParticipantId == receiverId
                     && e.Chat.SecondParticipantId == issuerId)
                    || (e.Chat.FirstParticipantId == issuerId
                        && e.Chat.SecondParticipantId == receiverId))
                .Include(e => e.Message)
                .Skip((page - 1) * length)
                .Take(length)
                .ToListAsync();
            return chatMessages;
        }
    }
}