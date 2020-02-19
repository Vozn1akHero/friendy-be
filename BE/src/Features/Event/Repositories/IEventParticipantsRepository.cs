using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.EventParticipantDtos;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventParticipantsRepository : IRepositoryBase<EventParticipants>
    {
        void AddParticipant(EventParticipants eventParticipants);
        Task<IEnumerable<ExemplaryEventParticipantDto>> GetExemplaryAsync(int eventId);

        Task<IEnumerable<EventParticipantForListDto>> GetRangeAsync(int eventId,
            int startIndex, int length);

        Task<IEnumerable<EventParticipantDetailedDto>> GetRangeDetailedAsync(int eventId,
            int startIndex, int length);

        Task<IEnumerable<EventParticipantDetailedDto>> FilterByKeywordAsync(
            string keyword);

        bool IsEventParticipant(int userId, int eventId);
        void DeleteByUserIdAndEventId(int userId, int eventId);
    }
}