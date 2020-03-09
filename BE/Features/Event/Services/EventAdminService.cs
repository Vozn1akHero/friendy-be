using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Services
{
    public interface IEventAdminService
    {
        Task<EventAdmins> CreateAndReturnAsync(int eventId, int userId);
        IEnumerable<EventAdminDto> FilterRangeByKeywordAndEventId(int eventId, string keyword,
            int page, int length);

        IEnumerable<EventAdminDto> GetRangeByEventIdWithPagination(int eventId, int page, int length);
    }

    public class EventAdminService : IEventAdminService
    {
        private readonly IRepositoryWrapper _repository;

        public EventAdminService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<EventAdmins> CreateAndReturnAsync(int eventId, int userId)
        {
            var entity = new EventAdmins
            {
                EventId = eventId,
                UserId = userId
            };
            _repository.EventAdmins.Create(entity);
            await _repository.SaveAsync();
            return entity;
        }

        public IEnumerable<EventAdminDto> FilterRangeByKeywordAndEventId(int eventId, string keyword, int page, int 
        length)
        {
            return _repository.EventAdmins.FilterRangeByEventIdAndKeyword(eventId, keyword, page, length,
                EventAdminDto.Selector);
        }

        public IEnumerable<EventAdminDto> GetRangeByEventIdWithPagination(int eventId, int page, int length)
        {
            return _repository.EventAdmins.GetRangeByEventIdWithPagination(eventId, page, length,
                EventAdminDto.Selector);
        }
    }
}