using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<string> GetChatUrlPartById(int chatId)
        {
            return await FindByCondition(e => e.Id == chatId).Select(e => e.UrlHash).SingleOrDefaultAsync();
        }
        
        public async Task<int> GetChatIdByUrlHash(string urlHash)
        {
            return await FindByCondition(e => e.UrlHash == urlHash).Select(e => e.Id).SingleOrDefaultAsync();
        }
    }
}