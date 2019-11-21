using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.Event
{
    public interface IEventParticipantsRepository : IRepositoryBase<EventParticipants>
    {
        Task<IEnumerable<ExemplaryEventParticipantDto>> GetExemplary(int eventId);
        Task<IEnumerable<EventParticipantForListDto>> GetRange(int eventId, int startIndex, int length);
    }
}