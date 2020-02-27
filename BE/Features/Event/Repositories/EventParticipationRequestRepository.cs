using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public class EventParticipationRequestRepository :
        RepositoryBase<EventParticipationRequest>, IEventParticipationRequestRepository
    {
        public EventParticipationRequestRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public void DeleteByRequestId(int id, int eventId)
        {
            var request = FindByCondition(e => e.Id == id && e.EventId == eventId).SingleOrDefault();
            if(request!=null) Delete(request);
        }

        public bool GetStatus(int issuerId, int eventId)
        {
            return ExistsByCondition(e => e.IssuerId == issuerId
                                          && e.EventId == eventId);
        }

        public void CreateRequest(EventParticipationRequest eventParticipationRequest)
        {
            Create(eventParticipationRequest);
        }

        public void DeleteRequest(EventParticipationRequest eventParticipationRequest)
        {
            var found = FindByCondition(e =>
                e.EventId == eventParticipationRequest.EventId &&
                e.IssuerId == eventParticipationRequest.IssuerId).SingleOrDefault();
            if (found != null) Delete(found);
        }

        public IEnumerable<TType> FindByKeyword<TType>(int eventId, string keyword,
            Expression<Func<EventParticipationRequest, TType>> selector)
        {
            return FindByCondition(e => e.EventId == eventId 
                                        && (e.Issuer.Name + e.Issuer.Surname)
                                        .Contains(keyword))
                .Select(selector)
                .ToList();
        }

        public IEnumerable<TType> GetWithPagination<TType>(int eventId, 
            int page,
            Expression<Func<EventParticipationRequest, TType>> selector)
        {
            int length = 20; 
            return FindByCondition(e => e.EventId == eventId)
                .Select(selector)
                .Skip((page-1)*length)
                .Take(length)
                .ToList();
        }

        public EventParticipationRequest FindById(int id)
        {
            return FindByCondition(e => e.Id == id).SingleOrDefault();
        }

        public void DeleteByIssuerId(int id, int eventId)
        {
            var req = FindByCondition(e => e.IssuerId == id && e.EventId == eventId)
                .SingleOrDefault();
            if(req!=null) Delete(req);
        }
    }
}