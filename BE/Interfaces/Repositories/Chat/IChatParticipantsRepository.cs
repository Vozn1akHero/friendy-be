using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatParticipantsRepository : IRepositoryBase<ChatParticipants>
    {
        Task AddNewAfterFriendAdding(int chatId, int[] participants);
        Task<List<ChatParticipants>> GetUserChatList(int userId);
    }
}