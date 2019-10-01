using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatParticipantsRepository : RepositoryBase<ChatParticipants>, IChatParticipantsRepository
    {
        public ChatParticipantsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<List<ChatParticipants>> GetUserChatList(int userId)
        {
            var chatList = await FindByCondition(e => e.UserId == userId).ToListAsync();
            return chatList;
        }
        
        public async Task AddNewAfterFriendAdding(int chatId, int[] participants)
        {
            int firstParticipant = participants[0];
            int secondParticipant = participants[1];
            Create(new ChatParticipants
            {
                ChatId = chatId,
                UserId = firstParticipant
            });
            Create(new ChatParticipants
            {
                ChatId = chatId,
                UserId = secondParticipant
            });
            await SaveAsync();
        }
    }
}