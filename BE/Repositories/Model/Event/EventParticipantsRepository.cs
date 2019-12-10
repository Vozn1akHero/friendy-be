using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Models;
using BE.Repositories.Interfaces.Event;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Event
{
    public class EventParticipantRepository : RepositoryBase<EventParticipants>, IEventParticipantsRepository
    {
        public EventParticipantRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<IEnumerable<ExemplaryEventParticipantDto>> GetExemplaryAsync(int eventId)
        {
            return await FindByCondition(e => e.EventId == eventId).Select(e => new ExemplaryEventParticipantDto()
            {
                Id = e.ParticipantId,
                AvatarPath = e.Participant.Avatar
            }).ToListAsync();
        }

        public async Task<bool> IsEventParticipant(int userId, int eventId)
        {
            return await Task.Run(() => ExistsByCondition(e => e.ParticipantId == userId && e.EventId == eventId));
        }

        public async Task Leave(int userId, int eventId)
        {
            var foundParticipant = await FindByCondition(e => e.ParticipantId == userId && e.EventId == eventId)
                .SingleOrDefaultAsync();
            if (foundParticipant != null)
            {
                Delete(foundParticipant);
                await SaveAsync();
            }
        }
        
        public async Task<IEnumerable<EventParticipantForListDto>> GetRangeAsync(int eventId, int startIndex, int length)
        {
            return await FindByCondition(e => e.EventId == eventId && e.EventId >= startIndex).Select(e => new EventParticipantForListDto
            {
                Id = e.ParticipantId,
                Name = e.Participant.Name,
                Surname = e.Participant.Surname,
                AvatarPath = e.Participant.Avatar
            }).Take(length).ToListAsync();
        }

        public async Task<IEnumerable<EventParticipantDetailedDto>> GetRangeDetailedAsync(int eventId, int startIndex, int length)
        {
            return await FindByCondition(e => e.EventId == eventId && e.EventId >= startIndex)
                .Select(e => new EventParticipantDetailedDto
                {
                    Id = e.ParticipantId,
                    Name = e.Participant.Name,
                    Surname = e.Participant.Surname,
                    AvatarPath = e.Participant.Avatar,
                    IsAdmin = e.Event.EventAdmins.SingleOrDefault(d => d.UserId == e.Id) != null
                }).Take(length).ToListAsync();
        }

        public async Task<IEnumerable<EventParticipantDetailedDto>> FilterByKeywordAsync(string keyword)
        {
            return await FindByCondition(e => (e.Participant.Name + " " + e.Participant.Surname).Contains(keyword))
                .Select(e => new EventParticipantDetailedDto
                {
                    Id = e.ParticipantId,
                    Name = e.Participant.Name,
                    Surname = e.Participant.Surname,
                    AvatarPath = e.Participant.Avatar,
                    IsAdmin = e.Event.EventAdmins.SingleOrDefault(d => d.UserId == e.Id) != null
                }).ToListAsync();
        }
    }
}