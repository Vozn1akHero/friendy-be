using MediatR;

namespace BE.CQRS.Commands.Event.EventCreation
{
    public class EventCreationCommand : IRequest
    {
        public Models.Event Event { get; set; }
        public int CreatorId { get; set; }
    }
}