using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.Event
{
    public interface IEventParticipantsRepository : IRepositoryBase<EventParticipants>
    {
        Task<IEnumerable<ExemplaryEventParticipantDto>> GetExemplaryAsync(int eventId);
        Task<IEnumerable<EventParticipantForListDto>> GetRangeAsync(int eventId, int startIndex, int length);
        Task<IEnumerable<EventParticipantDetailedDto>> GetRangeDetailedAsync(int eventId, int startIndex, int length);
        Task<IEnumerable<EventParticipantDetailedDto>> FilterByKeywordAsync(string keyword);
        
        Task<bool> IsEventParticipant(int userId, int eventId);
        Task Leave(int userId, int eventId);
    }
}