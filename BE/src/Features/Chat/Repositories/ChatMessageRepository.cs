using BE.Models;
using BE.Repositories;

namespace BE.Features.Chat.Repositories
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>,
        IChatMessageRepository
    {
        public ChatMessageRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public void Add(ChatMessage chatMessage)
        {
            Create(chatMessage);
        }
    }
}