using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.EventDtos;
using BE.Interfaces;

namespace BE.Services.Model
{
    public interface IEventSearchService
    {
        Task<IEnumerable<EventDto>> FilterParticipatingByKeywordAndUserId(int userId, string keyword);
        Task<IEnumerable<EventDto>> FilterAdministeredByKeywordAndUserId(int userId, string keyword);
        Task<IEnumerable<EventDto>> FilterByKeyword(string keyword);
    }
    
    public class EventSearchService : IEventSearchService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        
        public EventSearchService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> FilterParticipatingByKeywordAndUserId(int userId, string keyword)
        {
            var events = await _repository.Event.FilterParticipatingByKeywordAndUserId(userId, keyword);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }
        
         public async Task<IEnumerable<EventDto>> FilterAdministeredByKeywordAndUserId(int userId, string keyword)
        {
            var events = await _repository.Event.FilterAdministeredByKeywordAndUserId(userId, keyword);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }
        
         public async Task<IEnumerable<EventDto>> FilterByKeyword(string keyword)
        {
            var events = await _repository.Event.SearchByKeyword(keyword);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }
    }
}