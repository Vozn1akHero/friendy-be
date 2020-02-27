using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface
        IEventParticipationRequestRepository : IRepositoryBase<EventParticipationRequest>
    {
        void CreateRequest(EventParticipationRequest eventParticipationRequest);
        void DeleteRequest(EventParticipationRequest eventParticipationRequest);
        void DeleteByRequestId(int id, int eventId);
        void DeleteByIssuerId(int id, int eventId);
        bool GetStatus(int issuerId, int eventId);
        IEnumerable<TType> FindByKeyword<TType>(int eventId,
            string keyword,
            Expression<Func<EventParticipationRequest, TType>> selector);
        IEnumerable<TType> GetWithPagination<TType>(int eventId, 
            int page,
            Expression<Func<EventParticipationRequest, TType>> selector);

        EventParticipationRequest FindById(int id);
    }
}