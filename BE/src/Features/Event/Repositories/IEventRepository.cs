using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventRepository : IRepositoryBase<Models.Event>
    {
        Task<Models.Event> CreateAndReturn(Models.Event @event);

        Task<TType> GetById<TType>(int id, Expression<Func<Models.Event,
            TType>> selector);

        Task<IEnumerable<Models.Event>> SearchByKeyword(string keyword);
        Task<bool> IsUserCreatorById(int id, int userId);
        Task<object> GetWithSelectedFields(int id, string[] selectedFields);

        Task<IEnumerable<Models.Event>> FilterParticipatingByKeywordAndUserId(int userId,
            string keyword);

        Task<IEnumerable<Models.Event>> FilterAdministeredByKeywordAndUserId(int userId,
            string keyword);

        Task<IEnumerable<Models.Event>> GetParticipatingByUserIdAsync(int userId);
        Task<IEnumerable<Models.Event>> GetAdministeredByUserIdAsync(int userId);
        Task<IEnumerable<Models.Event>> GetClosestAsync(string city, int length, int issuerId);
        void UpdateById<TType>(int id, TType entry);
    }
}