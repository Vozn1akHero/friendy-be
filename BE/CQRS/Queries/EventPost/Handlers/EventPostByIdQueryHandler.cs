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
    public class EventPostByIdQueryHandler : IRequestHandler<GetEventPostById, IEnumerable<EventPostDto>>
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public EventPostByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<EventPostDto>> Handle(GetEventPostById request,
            CancellationToken cancellationToken)
        {
            var eventPost = await _repository.EventPost.GetByIdAsync(request.PostId);
            var eventPostDto =
                _mapper.Map<IEnumerable<EventPostDto>>(eventPost, opt => opt.Items["userId"] = request.UserId);
            return eventPostDto;
        }
    }
}