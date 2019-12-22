using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos;
using BE.Interfaces;
using MediatR;

namespace BE.Queries.EventPost
{
    public class EventPostRangeByIdQueryHandler : IRequestHandler<GetEventPostRangeById,
        IEnumerable<EventPostDto>>
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public EventPostRangeByIdQueryHandler(IRepositoryWrapper repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventPostDto>> Handle(GetEventPostRangeById request,
            CancellationToken cancellationToken)
        {
            var eventPosts = await _repository.EventPost.GetRangeByIdAsync(request.EventId, 
                request.StartIndex,
                request.Length);
            var eventPostsDtos = _mapper.Map<IEnumerable<EventPostDto>>(eventPosts,
                opt => opt.Items["userId"] = request.UserId);
            return eventPostsDtos;
        }
    }
}
