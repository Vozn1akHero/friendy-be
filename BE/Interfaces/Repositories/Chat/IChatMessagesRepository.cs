using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatMessagesRepository: IRepositoryBase<ChatMessages>
    {
        Task Add(int chatId, int messageId);
        Task<List<ChatMessages>> GetLastChatMessages(List<int> chatIdList);
        Task<List<ChatMessageDto>> GetByChatId(int chatId, int userId);
    }
}