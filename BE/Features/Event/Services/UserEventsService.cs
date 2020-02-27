using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Event.Dtos;
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
            var user = await _repository.User.GetSelectedFieldsById(id, e => new Models.User
            {
                City = e.City
            });
            string city = user.City.Title;
            var events = await _repository.Event.GetClosestAsync(city, length, id);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }
    }
}