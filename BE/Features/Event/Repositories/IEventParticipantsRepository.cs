using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventParticipantsRepository : IRepositoryBase<EventParticipants>
    {
        //void AddParticipant(EventParticipants eventParticipants);
        /*Task<IEnumerable<ExemplaryEventParticipantDto>> GetExemplaryAsync(int eventId);

        Task<IEnumerable<EventParticipantForListDto>> GetRangeAsync(int eventId,
            int startIndex, int length);

        Task<IEnumerable<EventParticipantDetailedDto>> GetRangeDetailedAsync(int eventId,
            int startIndex, int length);

        Task<IEnumerable<EventParticipantDetailedDto>> FilterByKeywordAsync(
            string keyword);*/
        IEnumerable<TType> FilterRangeByEventIdAndKeyword<TType>(int eventId,
            string keyword, int page, int length,
            Expression<Func<EventParticipants, TType>> selector);

        IEnumerable<TType> GetRangeByEventId<TType>(int eventId, int page, int length,
            Expression<Func<EventParticipants, TType>> selector);
        
        bool IsEventParticipant(int userId, int eventId);
        void DeleteByUserIdAndEventId(int userId, int eventId);
    }
}