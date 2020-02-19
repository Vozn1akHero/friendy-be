using BE.Features.Event.Helpers;
using BE.Repositories;

namespace BE.Features.Event.Services
{
    public interface IEventParticipationStatusService
    {
        EventParticipationStatusRes Get(int eventId, int issuerId);
    }

    public class EventParticipationStatusService : IEventParticipationStatusService
    {
        private readonly IRepositoryWrapper _repository;
        private int _eventId;
        private EventParticipationStatusRes _eventParticipationStatusRes;
        private int _issuerId;

        public EventParticipationStatusService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public EventParticipationStatusRes Get(int eventId, int issuerId)
        {
            Init(eventId, issuerId);

            if (IsParticipantAsync())
                _eventParticipationStatusRes.EventParticipationStatus =
                    EventParticipationStatus.Participant;
            else if (IsBanned())
                _eventParticipationStatusRes.EventParticipationStatus =
                    EventParticipationStatus.Banned;
            else if (IsRequestSent())
                _eventParticipationStatusRes.EventParticipationStatus =
                    EventParticipationStatus.RequestSent;
            return _eventParticipationStatusRes;
        }

        private void Init(int eventId, int issuerId)
        {
            _eventId = eventId;
            _issuerId = issuerId;
            _eventParticipationStatusRes = new EventParticipationStatusRes
            {
                EventId = eventId,
                IssuerId = issuerId,
                EventParticipationStatus = EventParticipationStatus.NonParticipant
            };
        }

        private bool IsParticipantAsync()
        {
            return _repository.EventParticipants.IsEventParticipant(_issuerId, _eventId);
        }

        private bool IsBanned()
        {
            return _repository.EventBannedUsers.IsBannedAsync(_issuerId, _eventId);
        }

        private bool IsRequestSent()
        {
            return _repository.EventParticipationRequest.GetStatus(_issuerId, _eventId);
        }
    }
}