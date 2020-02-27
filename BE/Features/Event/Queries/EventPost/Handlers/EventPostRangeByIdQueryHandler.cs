using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Post.Dtos;
using BE.Repositories;
using MediatR;

namespace BE.Features.Event.Queries.EventPost.Handlers
{
    public class EventPostRangeByIdQueryHandler : IRequestHandler<GetEventPostRangeById,
        IEnumerable<EventPostDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public EventPostRangeByIdQueryHandler(IRepositoryWrapper repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventPostDto>> Handle(GetEventPostRangeById request,
            CancellationToken cancellationToken)
        {
            var eventPosts = await _repository.EventPost.GetRangeByIdAsync(
                request.EventId,
                request.StartIndex,
                request.Length);
            var eventPostsDtos = _mapper.Map<IEnumerable<EventPostDto>>(eventPosts,
                opt => opt.Items["userId"] = request.UserId);
            return eventPostsDtos;
        }
    }
}