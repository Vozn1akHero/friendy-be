using System.Threading;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using MediatR;

namespace BE.Features.Event.Commands.Event.EventCreation
{
    public class EventCreationCommandHandler :
        IRequestHandler<EventCreationCommand>
    {
        private readonly IRepositoryWrapper _repository;

        public EventCreationCommandHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(EventCreationCommand request,
            CancellationToken cancellationToken)
        {
            request.Event.CreatorId = request.CreatorId;
            request.Event.Avatar = "wwwroot/EventAvatar/default-event-avatar.png";
            request.Event.Background =
                "wwwroot/EventBackground/default-event-background.png";
            var eventData = await _repository.Event.CreateAndReturn(request.Event);
            _repository.EventParticipants.AddParticipant(new EventParticipants
            {
                EventId = eventData.Id,
                ParticipantId = request.CreatorId
            });
            await _repository.EventAdmins.CreateAndReturn(eventData.Id,
                request.CreatorId);
            await _repository.SaveAsync();
            return Unit.Value;
        }
    }
}