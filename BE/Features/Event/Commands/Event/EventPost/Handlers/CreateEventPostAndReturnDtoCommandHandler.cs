using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Post.Dtos;
using BE.Repositories;
using MediatR;

namespace BE.Features.Event.Commands.Event.EventPost.Handlers
{
    public class CreateAndReturnDtoCommandHandler : IRequestHandler<
        CreateEventPostAndReturnDtoCommand, EventPostDto>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CreateAndReturnDtoCommandHandler(IRepositoryWrapper repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventPostDto> Handle(CreateEventPostAndReturnDtoCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.EventPost.CreateAsync(request.EventPost);
            var newEventPost =
                await _repository.EventPost.GetByIdAsync(request.EventPost.Id);
            var newEventPostDto =
                _mapper.Map<EventPostDto>(newEventPost,
                    opt => opt.Items["userId"] = null);
            return newEventPostDto;
        }
    }
}