using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatMessagesRepository: IRepositoryBase<ChatMessages>
    {
        Task Add(ChatMessages chatMessages);
        //Task<List<ChatMessages>> GetLastChatMessages(List<int> chatIdList);
        //Task<List<ChatLastMessageDto>> GetLastChatMessageRangeByReceiverId(int receiverId, int startIndex, int length);
        //Task<List<ChatMessageDto>> GetByChatId(int chatId, int userId);
        Task<IEnumerable<ChatMessages>> GetMessageRangeByReceiverId(int
            receiverId, int
            issuerId, int startIndex, int length);
    }
}