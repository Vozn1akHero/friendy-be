using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Event.Dtos;
using BE.Features.Search.Dtos;
using BE.Repositories;
using EventDto = BE.Features.Event.Dtos.EventDto;

namespace BE.Features.Search.Services
{
    public interface IEventSearchService
    {
        Task<IEnumerable<EventDto>> FilterParticipatingByKeywordAndUserId(int userId,
            string keyword);

        Task<IEnumerable<EventDto>> FilterAdministeredByKeywordAndUserId(int userId,
            string keyword);

        Task<IEnumerable<CommonEventDto>> FilterAllByKeywordAsync(int issuerId, string keyword, int page, int
            size);
        IEnumerable<EventDto> FilterByCriteria(EventSearchDto eventSearchDto);
    }

    public class EventSearchService : IEventSearchService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public EventSearchService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> FilterParticipatingByKeywordAndUserId(
            int userId, string keyword)
        {
            var events =
                await _repository.Event.FilterParticipatingByKeywordAndUserId(userId,
                    keyword);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }

        public async Task<IEnumerable<EventDto>> FilterAdministeredByKeywordAndUserId(
            int userId, string keyword)
        {
            var events =
                await _repository.Event.FilterAdministeredByKeywordAndUserId(userId,
                    keyword);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return eventDtos;
        }

        public async Task<IEnumerable<CommonEventDto>> FilterAllByKeywordAsync(int issuerId, string keyword, int page, int 
        size)
        {
            var events = await _repository.Event.SearchByKeywordAsync(keyword, page, size, CommonEventDto.Selector(issuerId));
            return events;
        }

        public IEnumerable<EventDto> FilterByCriteria(EventSearchDto eventSearchDto)
        {
            //var events = _eventDetailedSearch.SearchByCriteria(eventSearchDto);
            //var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return null;
        }
    }
}