using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;

namespace BE.Repositories.Chat
{
    public class ChatRepository: RepositoryBase<Models.Chat>, IChatRepository
    {
        public ChatRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<Models.Chat> AddNewAfterFriendAdding()
        {
            byte[] bytes = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            string newChatHashUrl = BitConverter.ToString(bytes);
            var newChat = new Models.Chat
            {
                UrlHash = newChatHashUrl
            };
            Create(newChat);
            await SaveAsync();
            return newChat;
        }
    }
}