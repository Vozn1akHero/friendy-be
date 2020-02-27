using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Post.Dtos;
using BE.Repositories;
using MediatR;

namespace BE.Features.Event.Queries.EventPost.Handlers
{
    public class
        EventPostByIdQueryHandler : IRequestHandler<GetEventPostById,
            IEnumerable<EventPostDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public EventPostByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventPostDto>> Handle(GetEventPostById request,
            CancellationToken cancellationToken)
        {
            var eventPost = await _repository.EventPost.GetByIdAsync(request.PostId);
            //var eventPost = GetFakeData().SingleOrDefault(e => e.Id == request.PostId);
            var eventPostDto =
                _mapper.Map<IEnumerable<EventPostDto>>(eventPost,
                    opt => opt.Items["userId"] = request.UserId);
            return eventPostDto;
        }

        public IEnumerable<Models.EventPost> GetFakeData()
        {
            return new List<Models.EventPost>
            {
                new Models.EventPost
                {
                    Id = 1, EventId = 1, PostId = 1, Post = new Models.Post
                    {
                        Id = 1, Content = "TEST1",
                        Date = new DateTime(2019, 11, 19, 20, 09, 38, 497),
                        ImagePath = null
                    }
                },
                new Models.EventPost
                {
                    Id = 2, EventId = 1, PostId = 2, Post = new Models.Post
                    {
                        Id = 2, Content = "TEST2",
                        Date = new DateTime(2019, 11, 20, 20, 09, 38, 497),
                        ImagePath = null
                    }
                }
            };
        }
    }
}