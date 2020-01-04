using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.EventDtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Services.Model
{
    public interface IEventDataService
    {
        Task<EventDto> GetDtoById(int eventId);
        Task UpdateAvatarAsync(int eventId, string path);
        Task UpdateBackgroundAsync(int eventId, string path);
    }
    public class EventDataService : IEventDataService
    {
        private readonly IRepositoryWrapper _repository;
        private FriendyContext _friendyContext;
        private readonly IMapper _mapper;

        public EventDataService(IRepositoryWrapper repository,
            IMapper mapper,
            FriendyContext friendyContext)
        {
            _repository = repository;
            _mapper = mapper;
            _friendyContext = friendyContext;
        }
        
        public async Task<EventDto> GetDtoById(int eventId)
        {
            var eventData = await _repository.Event.GetById(eventId);
            var eventDataDto = _mapper.Map<EventDto>(eventData);
            return eventDataDto;
        }

        public async Task UpdateAvatarAsync(int eventId, string path)
        {
            var @event = await _repository.Event.GetById(eventId);
            @event.Avatar = path;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateBackgroundAsync(int eventId, string path)
        {
            var @event = await _repository.Event.GetById(eventId);
            @event.Background = path;
            await _friendyContext.SaveChangesAsync();
        }
    }
}