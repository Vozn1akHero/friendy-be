using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Event.Dto;
using BE.Features.Event.Helpers;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Services
{
    public interface IEventParticipationRequestService
    {
        Task<EventParticipationRequestResponse> CreateAndReturnAsync(int issuerId,
            int eventId);

        Task DeleteAsync(int issuerId, int eventId);
        Task DeleteByIssuerId(int issuerId, int eventId);
        Task ConfirmByIssuerId(int issuerId, int eventId);

        IEnumerable<EventParticipationRequestDto>
            FindByKeyword(int eventId, string keyword);

        IEnumerable<EventParticipationRequestDto>
            GetWithPagination(int eventId, int page);
    }

    public class EventParticipationRequestService : IEventParticipationRequestService
    {
        private readonly IRepositoryWrapper _repository;

        public EventParticipationRequestService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<EventParticipationRequestResponse> CreateAndReturnAsync(
            int issuerId, int eventId)
        {
            var isEventAdmin =
                await _repository.EventAdmins.IsUserAdminById(eventId, issuerId);
            if (isEventAdmin)
            {
                var newParticipant = new EventParticipants
                    {EventId = eventId, ParticipantId = issuerId};
                _repository.EventParticipants.AddParticipant(newParticipant);
                await _repository.SaveAsync();
                return new EventParticipationRequestResponse
                {
                    CreationResult =
                        EventParticipationRequestCreationResult.RequestByAdmin,
                    CreatedEntity = newParticipant
                };
            }

            var newRequest = new EventParticipationRequest
            {
                IssuerId = issuerId,
                EventId = eventId
            };
            _repository.EventParticipationRequest.CreateRequest(newRequest);
            await _repository.SaveAsync();
            return new EventParticipationRequestResponse
            {
                CreationResult = EventParticipationRequestCreationResult.CreatedForUser,
                CreatedEntity = newRequest
            };
        }

        public async Task DeleteAsync(int issuerId, int eventId)
        {
            var newRequest = new EventParticipationRequest
            {
                IssuerId = issuerId,
                EventId = eventId
            };
            _repository.EventParticipationRequest.DeleteRequest(newRequest);
            await _repository.EventParticipationRequest.SaveAsync();
        }

        public async Task DeleteByIssuerId(int issuerId, int eventId)
        {
            _repository.EventParticipationRequest.DeleteByIssuerId(issuerId,eventId);
            await _repository.SaveAsync();
        }

        public async Task ConfirmByIssuerId(int issuerId, int eventId)
        {
            //var req = _repository.EventParticipationRequest.FindById(requestId);
            _repository.EventParticipationRequest.DeleteByIssuerId(issuerId, eventId);
            _repository.EventParticipants.AddParticipant(new EventParticipants()
            {
                ParticipantId = issuerId,
                EventId = eventId
            });
            await _repository.SaveAsync();
        }

        public IEnumerable<EventParticipationRequestDto> FindByKeyword(int 
        eventId, string keyword)
        {
            return _repository.EventParticipationRequest.FindByKeyword(eventId,
                keyword, EventParticipationRequestDto.Selector());
        }

        public IEnumerable<EventParticipationRequestDto> GetWithPagination(int eventId, int page)
        {
            return _repository.EventParticipationRequest.GetWithPagination(eventId,
                page, EventParticipationRequestDto.Selector());
        }
    }
}