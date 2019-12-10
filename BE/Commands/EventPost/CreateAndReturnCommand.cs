using BE.Dtos;
using MediatR;

namespace BE.Commands.EventPost
{
    public class CreateEventPostAndReturnDtoCommand : IRequest<EventPostDto>
    {
        public Models.EventPost EventPost { get; set; }
    }
}