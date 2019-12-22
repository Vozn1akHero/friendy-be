using System.Collections.Generic;
using BE.Dtos;
using MediatR;

namespace BE.Queries.EventPost
{
    public class GetEventPostById : IRequest<IEnumerable<EventPostDto>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}