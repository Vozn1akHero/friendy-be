using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;

namespace BE.Repositories.Chat
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task Add(ChatMessage chatMessage)
        {
            Create(chatMessage);
            await SaveAsync();
        }
    }
}