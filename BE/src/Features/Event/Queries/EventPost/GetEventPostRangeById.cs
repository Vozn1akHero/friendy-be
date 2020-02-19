using System.Collections.Generic;
using BE.Dtos.PostDtos;
using MediatR;

namespace BE.Features.Event.Queries.EventPost
{
    public class GetEventPostRangeById : IRequest<IEnumerable<EventPostDto>>
    {
        public int EventId { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public int UserId { get; set; }
    }
}