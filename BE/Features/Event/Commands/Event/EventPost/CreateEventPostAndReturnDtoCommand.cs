using BE.Features.Post.Dtos;
using MediatR;

namespace BE.Features.Event.Commands.Event.EventPost
{
    public class CreateEventPostAndReturnDtoCommand : IRequest<EventPostDto>
    {
        public Models.EventPost EventPost { get; set; }
    }
}