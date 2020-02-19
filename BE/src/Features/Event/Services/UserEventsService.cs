using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Event.Dto;
using BE.Repositories;

namespace BE.Features.Event.Services
{
    public interface IUserEventsService
    {
        Task<IEnumerable<EventDto>> GetParticipatingByIdAsync(int id);
        Task<IEnumerable<EventDto>> GetAdministeredByIdAsync(int id);
        Task<IEnumerable<EventDto>> GetClosestByUserIdAsync(int id, int length);
    }

    public class UserEventsService : IUserEventsService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UserEventsService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> GetParticipatingByIdAsync(int id)
        {
            var events = await _repository.Event.GetParticipatingByUserIdAsync(id);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }

        public async Task<IEnumerable<EventDto>> GetAdministeredByIdAsync(int id)
        {
            var events = await _repository.Event.GetAdministeredByUserIdAsync(id);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }

        public async Task<IEnumerable<EventDto>> GetClosestByUserIdAsync(int id,
            int length)
        {
            var user = (IDictionary<string, object>) await _repository
                .User
                .GetWithSelectedFields(id, new[] {"City"});
            string city = user["City"].ToString();
            var events = await _repository.Event.GetClosestAsync(city, length, id);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }
    }
}