using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Interfaces;

namespace BE.Services.Model
{
    public interface IUserEventsService
    {
        Task<IEnumerable<EventDto>> GetParticipatingByIdAsync(int id);
        Task<IEnumerable<EventDto>> GetAdministeredByIdAsync(int id);
        Task<IEnumerable<EventDto>> GetClosestByUserIdAsync(int id, int length);
    }
    
    public class UserEventsService : IUserEventsService
    {
        private IRepositoryWrapper _repository;

        public UserEventsService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EventDto>> GetParticipatingByIdAsync(int id)
        {
            var events = await _repository.Event.GetParticipatingByUserIdAsync(id);
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Street = e.Street,
                StreetNumber = e.StreetNumber,
                City = e.City,
                AvatarPath = e.Avatar,
                ParticipantsAmount = e.ParticipantsAmount,
                CurrentParticipantsAmount = e.EventParticipants.Count,
                Date = e.Date
            });
            return eventDtos;
        }

        public async Task<IEnumerable<EventDto>> GetAdministeredByIdAsync(int id)
        {
            var events = await _repository.Event.GetAdministeredByUserIdAsync(id);
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Street = e.Street,
                StreetNumber = e.StreetNumber,
                City = e.City,
                AvatarPath = e.Avatar,
                ParticipantsAmount = e.ParticipantsAmount,
                CurrentParticipantsAmount = e.EventParticipants.Count,
                Date = e.Date
            });
            return eventDtos;
        }
        
        public async Task<IEnumerable<EventDto>> GetClosestByUserIdAsync(int id, int length)
        {
            var user = await _repository.User.GetByIdAsync(id);
            var events = await _repository.Event.GetClosestAsync(user.City, length);
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Street = e.Street,
                StreetNumber = e.StreetNumber,
                City = e.City,
                AvatarPath = e.Avatar,
                ParticipantsAmount = e.ParticipantsAmount,
                CurrentParticipantsAmount = e.EventParticipants.Count,
                Date = e.Date
            });
            return eventDtos;
        }
    }
}