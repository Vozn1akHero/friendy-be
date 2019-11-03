using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatParticipantsRepository : IRepositoryBase<ChatParticipants>
    {
        Task AddNewAfterFriendAdding(int chatId, int[] participants);
        Task<List<ChatParticipants>> GetUserChatList(int userId);
        Task<List<ParticipantsBasicDataDto>> GetParticipantsBasicDataByChatId(int chatId);
        Task<FriendBasicDataInDialogDto> GetFriendBasicDataInDialogByChatId(int chatId, int userId);
    }
}