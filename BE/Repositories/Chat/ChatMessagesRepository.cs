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
        {
        }

        public async Task<List<ChatMessages>> GetLastChatMessages(List<int> chatIdList)
        {
            var chatMessages = new List<ChatMessages>();
            chatIdList.ForEach(async id =>
            {
                var messages = await FindByCondition(e => e.ChatId == id)
                    .Include(e => e.Message)
                    .Include(e => e.Message.User)
                    .OrderByDescending(e => e.Message.Date)
                    .FirstOrDefaultAsync();
                chatMessages.Add(messages);
            });
            return chatMessages;
        }
    }
}