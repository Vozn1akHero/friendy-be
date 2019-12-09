using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;

namespace BE.Queries.EventPost
{
    public class EventPostQueryHandler
    {
        private IRepositoryWrapper _repository;

        public EventPostQueryHandler(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EventPostOnWallDto>> Handle(GetEventPostRangeById query)
        {
            var eventPosts = await _repository.EventPost.GetRangeByIdAsync(query.EventId, query.StartIndex, query.Length, query.UserId);
            return eventPosts;
        }
    }
}