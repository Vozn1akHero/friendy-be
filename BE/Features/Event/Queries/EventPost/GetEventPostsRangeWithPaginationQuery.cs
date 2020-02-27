using System.Collections.Generic;
using BE.Features.Post.Dtos;
using MediatR;

namespace BE.Features.Event.Queries.EventPost
{
    public class
        GetEventPostsRangeWithPaginationQuery : IRequest<IEnumerable<EventPostDto>>
    {
        public int EventId { get; set; }
        public int Page { get; set; }
        public int UserId { get; set; }
    }
}