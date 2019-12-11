using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces.Event;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Model.Event
{
    public class EventParticipationRequestRepository : RepositoryBase<EventParticipationRequest>, IEventParticipationRequestRepository
    {
        public EventParticipationRequestRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task CreateAsync(EventParticipationRequest eventParticipationRequest)
        {
            Create(eventParticipationRequest);
            await SaveAsync();
        }

        public async Task DeleteAsync(EventParticipationRequest eventParticipationRequest)
        {
            var found = await FindByCondition(e =>
                e.EventId == eventParticipationRequest.EventId && e.IssuerId == eventParticipationRequest.IssuerId).SingleOrDefaultAsync();
            if (found != null)
            {
                Delete(found);
                await SaveAsync();
            }
        }

        public async Task<bool> GetStatus(int issuerId, int eventId)
        {
            return await Task.Run(() => ExistsByCondition(e => e.IssuerId == issuerId && e.EventId == eventId));
        }
    }
}