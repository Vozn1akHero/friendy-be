using System.Collections.Generic;
using BE.Features.Post.Dtos;
using MediatR;

namespace BE.Features.Event.Queries.EventPost
{
    public class GetEventPostById : IRequest<IEnumerable<EventPostDto>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}