using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.EventDtos;
using BE.Interfaces;

namespace BE.Services.Model
{
    public interface IEventDataService
    {
        Task<EventDto> GetDtoById(int eventId);
    }
    public class EventDataService : IEventDataService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public EventDataService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<EventDto> GetDtoById(int eventId)
        {
            var eventData = await _repository.Event.GetById(eventId);
            var eventDataDto = _mapper.Map<EventDto>(eventData);
            return eventDataDto;
        }
    }
}