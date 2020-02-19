using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Chat.Repositories
{
    public class ChatRepository : RepositoryBase<Models.Chat>, IChatRepository
    {
        public ChatRepository(FriendyContext friendyContext)
            : base(friendyContext)
        {
        }

        public async Task Add(int firstParticipantId, int secondParticipantId)
        {
            var newChat = new Models.Chat
            {
                FirstParticipantId = firstParticipantId,
                SecondParticipantId = secondParticipantId
            };
            Create(newChat);
            await SaveAsync();
        }

        public async Task<TType> GetByInterlocutorsIdentifiers<TType>(
            int firstParticipantId, int secondParticipantId, 
            Expression<Func<Models.Chat, TType>> selector)
        {
            return await FindByCondition(e =>
                    e.FirstParticipantId == firstParticipantId &&
                    e.SecondParticipantId == secondParticipantId
                    || e.FirstParticipantId == secondParticipantId &&
                    e.SecondParticipantId == firstParticipantId)
                .Select(selector)
                .SingleOrDefaultAsync();
        }
    }
}