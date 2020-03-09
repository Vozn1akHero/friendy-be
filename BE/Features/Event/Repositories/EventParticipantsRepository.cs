using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Event.Repositories
{
    public class EventParticipantsRepository : RepositoryBase<EventParticipants>,
        IEventParticipantsRepository
    {
        public EventParticipantsRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public void AddParticipant(EventParticipants eventParticipants)
        {
            Create(eventParticipants);
        }

        public bool IsEventParticipant(int userId, int eventId)
        {
            return ExistsByCondition(e =>
                e.ParticipantId == userId && e.EventId == eventId);
        }

        public void DeleteByUserIdAndEventId(int userId, int eventId)
        {
            var foundParticipant = FindByCondition(e => e.ParticipantId == userId
                                                        && e.EventId == eventId)
                .SingleOrDefault();
            if (foundParticipant != null) Delete(foundParticipant);
        }

        public IEnumerable<TType> GetRangeByEventId<TType>(int eventId, int page, int length,
            Expression<Func<EventParticipants, TType>> selector)
        {
            return FindByCondition(e => e.EventId == eventId).Select(selector)
                .Skip((page-1)*length)
                .Take(length)
                .ToList();
        }

        public IEnumerable<TType> FilterRangeByEventIdAndKeyword<TType>(int eventId,
            string keyword, int page, int length,
            Expression<Func<EventParticipants, TType>> selector)
        {
            return FindByCondition(e =>
                    (e.Participant.Name + " " + e.Participant.Surname).Contains(keyword))
                .Skip((page-1)*length)
                .Take(length)
                .Select(selector)
                .ToList();
        }
    }
}