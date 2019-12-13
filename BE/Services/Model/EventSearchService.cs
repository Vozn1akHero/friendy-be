using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.EventDtos;
using BE.Interfaces;
using BE.Services.Elasticsearch;

namespace BE.Services.Model
{
    public interface IEventSearchService
    {
        Task<IEnumerable<EventDto>> FilterParticipatingByKeywordAndUserId(int userId, string keyword);
        Task<IEnumerable<EventDto>> FilterAdministeredByKeywordAndUserId(int userId, string keyword);
        Task<IEnumerable<EventDto>> FilterByKeyword(string keyword);
        IEnumerable<EventDto> FilterByCriteria(EventSearchDto eventSearchDto);
    }
    
    public class EventSearchService : IEventSearchService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IEventDetailedSearch _eventDetailedSearch;

        public EventSearchService(IRepositoryWrapper repository, IMapper mapper, IEventDetailedSearch eventDetailedSearch)
        {
            _repository = repository;
            _mapper = mapper;
            _eventDetailedSearch = eventDetailedSearch;
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

         public IEnumerable<EventDto> FilterByCriteria(EventSearchDto eventSearchDto)
         {
             var events = _eventDetailedSearch.SearchByCriteria(eventSearchDto);
             return events;
         }
    }
}