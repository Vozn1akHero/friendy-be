using BE.Interfaces.Repositories.Chat;
using BE.Models;

namespace BE.Repositories.Chat
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
    }
}