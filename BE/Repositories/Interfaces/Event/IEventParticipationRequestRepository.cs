using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.Event
{
    public interface IEventParticipationRequestRepository : IRepositoryBase<EventParticipationRequest>
    {
        Task CreateAsync(EventParticipationRequest eventParticipationRequest);
        Task DeleteAsync(EventParticipationRequest eventParticipationRequest);
        Task<bool> GetStatus(int issuerId, int eventId);
    }
}