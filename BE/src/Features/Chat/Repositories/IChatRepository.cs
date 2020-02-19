using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Repositories;

namespace BE.Features.Chat.Repositories
{
    public interface IChatRepository : IRepositoryBase<Models.Chat>
    {
        Task Add(int firstParticipantId, int secondParticipantId);

        Task<TType> GetByInterlocutorsIdentifiers<TType>(int firstParticipantId,
            int secondParticipantId, Expression<Func<Models.Chat, TType>> selector);
    }
}