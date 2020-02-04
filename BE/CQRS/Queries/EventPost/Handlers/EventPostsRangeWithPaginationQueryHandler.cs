using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos;
using BE.Interfaces;
using MediatR;

namespace BE.Queries.EventPost
{
    public class EventPostsRangeWithPaginationQueryHandler : IRequestHandler<GetEventPostsRangeWithPaginationQuery,
        IEnumerable<EventPostDto>>
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public EventPostsRangeWithPaginationQueryHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<EventPostDto>> Handle(GetEventPostsRangeWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var eventPosts = await _repository.EventPost.GetLastRangeByIdWithPaginationAsync(request.EventId, 
                request.Page);
            var eventPostsDtos = _mapper.Map<IEnumerable<EventPostDto>>(eventPosts,
                opt => opt.Items["userId"] = request.UserId);
            return eventPostsDtos;
        }
    }
}