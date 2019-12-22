using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos;
using BE.Interfaces;
using BE.Repositories;
using MediatR;

namespace BE.Commands.EventPost.Handlers
{
    public class CreateAndReturnDtoCommandHandler : IRequestHandler<CreateEventPostAndReturnDtoCommand, EventPostDto>
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public CreateAndReturnDtoCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventPostDto> Handle(CreateEventPostAndReturnDtoCommand request, CancellationToken cancellationToken)
        {
            await _repository.EventPost.CreateAsync(request.EventPost);
            var newEventPost = await _repository.EventPost.GetByIdAsync(request.EventPost.Id);
            var newEventPostDto = _mapper.Map<EventPostDto>(newEventPost, opt => opt.Items["userId"] = null); 
            return newEventPostDto;
        }
    }
}