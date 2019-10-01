using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatMessagesRepository: IRepositoryBase<ChatMessages>
    {
        Task<List<ChatMessages>> GetLastChatMessages(List<int> chatIdList);
    }
}