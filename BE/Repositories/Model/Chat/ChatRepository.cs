using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatRepository: RepositoryBase<Models.Chat>, IChatRepository
    {

        public ChatRepository(FriendyContext friendyContext)
             : base(friendyContext)
        { }

        public async Task Add(int firstParticipantId, int secondParticipantId)
        {
            var newChat = new Models.Chat()
            {
                FirstParticipantId = firstParticipantId,
                SecondParticipantId = secondParticipantId
            };
            Create(newChat);
            await SaveAsync();
        }

        public async Task<Models.Chat> GetByInterlocutorsIdentifiers(int firstParticipantId, int secondParticipantId)
        {
            return await FindByCondition(e =>
                    e.FirstParticipantId == firstParticipantId && e.SecondParticipantId == secondParticipantId
                    || e.FirstParticipantId == secondParticipantId && e.SecondParticipantId == firstParticipantId)
                .SingleOrDefaultAsync();
        }
        
    }
}