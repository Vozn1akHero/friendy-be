using BE.Models;
using BE.Repositories;

namespace BE.Features.Chat.Repositories
{
    public interface IChatMessageRepository : IRepositoryBase<ChatMessage>
    {
        void Add(ChatMessage chatMessage);
    }
}