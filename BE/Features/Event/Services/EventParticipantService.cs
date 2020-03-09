using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Event.Dtos;
using BE.Features.Event.Helpers;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Services
{
    public interface IEventParticipantService
    {
        IEnumerable<EventParticipantDetailedDto> GetRangeByEventIdWithPagination(int eventId,
            int page, int length, int issuerId);
        Task<IEnumerable<BannedEventParticipantDto>>
            FindBannedUsersByEventIdAsync(int eventId);
        IEnumerable<EventParticipantDetailedDto> FilterRangeByKeywordAndEventId(int eventId, string keyword,
            int page, int length, int issuerId);
        void BanParticipant(int id, int eventId);
        void UnbanParticipant(int id, int eventId);
        Task LeaveAsync(int issuerId, int eventId);
        EventParticipationStatusRes Status(int eventId, int issuerId);
    }

    public class EventParticipantService : IEventParticipantService
    {
        private readonly IEventParticipationStatusService
            _eventParticipationStatusService;
        private readonly IRepositoryWrapper _repository;

        public EventParticipantService(IRepositoryWrapper repository,
            IEventParticipationStatusService eventParticipationStatusService)
        {
            _repository = repository;
            _eventParticipationStatusService = eventParticipationStatusService;
        }

        public IEnumerable<EventParticipantDetailedDto> GetRangeByEventIdWithPagination(int eventId,
            int page, int length, int issuerId)
        {
            var participants = _repository.EventParticipants.GetRangeByEventId(eventId, page, length, 
            EventParticipantDetailedDto.Selector);
            return participants;
        }

        public IEnumerable<EventParticipantDetailedDto> FilterRangeByKeywordAndEventId(int eventId, string keyword,
         int
            page, int length, int issuerId)
        {
            var participants = _repository.EventParticipants.FilterRangeByEventIdAndKeyword(eventId, keyword, page, length,
                EventParticipantDetailedDto.Selector);
            return participants;
        }

        public async Task<IEnumerable<BannedEventParticipantDto>>
            FindBannedUsersByEventIdAsync(int eventId)
        {
            var bu = await _repository.EventBannedUsers
                .FindEventBannedUsersByEventIdAsync(eventId);
            var res = bu.Select(e => new BannedEventParticipantDto
            {
                Id = e.Id,
                Name = e.User.Name,
                Surname = e.User.Surname,
                AvatarPath = e.User.Avatar
            });
            return res;
        }

        public void UnbanParticipant(int id, int eventId)
        {
            _repository.EventBannedUsers.RemoveByUserIdAndEventId(id, eventId);
            _repository.Save();
        }

        public async Task LeaveAsync(int issuerId, int eventId)
        {
            _repository.EventParticipants.DeleteByUserIdAndEventId(issuerId, eventId);
            await _repository.SaveAsync();
        }

        public EventParticipationStatusRes Status(int eventId, int issuerId)
        {
            return _eventParticipationStatusService.Get(eventId, issuerId);
        }

        public void BanParticipant(int id, int eventId)
        {
            _repository.EventBannedUsers.Add(new EventBannedUsers
            {
                EventId = eventId,
                UserId = id
            });
            _repository
                .EventParticipants
                .DeleteByUserIdAndEventId(id, eventId);
            _repository.Save();
        }

        public async Task LeaveEvent(int userId, int eventId)
        {
        }
    }
}