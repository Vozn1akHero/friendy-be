using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Interfaces;
using BE.Models;
using MediatR;

namespace BE.CQRS.Commands.Event.EventCreation
{
    public class EventCreationCommandHandler : 
    IRequestHandler<EventCreationCommand>
    {
        private IRepositoryWrapper _repository;

        public EventCreationCommandHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(EventCreationCommand request, 
        CancellationToken cancellationToken)
        {
            request.Event.CreatorId = request.CreatorId;
            request.Event.Avatar = "wwwroot/EventAvatar/default-event-avatar.png";
            request.Event.Background = "wwwroot/EventBackground/default-event-background.png";
            var eventData = await _repository.Event.CreateAndReturn(request.Event);
            await _repository.EventParticipants.Add(new EventParticipants
            {
                EventId = eventData.Id,
                ParticipantId = request.CreatorId
            });
            await _repository.EventAdmins.CreateAndReturn(eventData.Id,
                request.CreatorId);
            return Unit.Value;
        }
    }
}