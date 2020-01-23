using System.Threading.Tasks;
using BE.Dtos.ChatDtos;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatRepository: IRepositoryBase<Models.Chat>
    {
        Task Add(int firstParticipantId, int secondParticipantId);
        Task<Models.Chat> GetByInterlocutorsIdentifiers(int firstParticipantId, int secondParticipantId);
    }
}