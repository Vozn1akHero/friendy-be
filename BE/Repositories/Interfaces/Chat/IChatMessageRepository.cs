using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatMessageRepository : IRepositoryBase<ChatMessage>
    {
        Task Add(ChatMessage chatMessage);
    }
}