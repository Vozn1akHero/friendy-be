using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Event.Repositories
{
    public class EventAdminsRepository : RepositoryBase<EventAdmins>,
        IEventAdminsRepository
    {
        public EventAdminsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task DeleteByEventIdAndAdminId(int eventId, int adminId)
        {
            var foundEntity =
                await FindByCondition(e => e.EventId == eventId && e.UserId == adminId)
                    .SingleOrDefaultAsync();
            if (foundEntity != null)
            {
                Delete(foundEntity);
                await SaveAsync();
            }
        }

        public IEnumerable<TType> FilterRangeByEventIdAndKeyword<TType>(int eventId, string keyword, int page, int length,
            Expression<Func<EventAdmins, TType>> selector)
        {
            return FindByCondition(e => e.EventId == eventId && (e.User.Name + e.User.Surname).Contains(keyword))
                .Select(selector)
                .Skip((page - 1) * length)
                .Take(length)
                .ToList();
        }

        public IEnumerable<TType> GetRangeByEventIdWithPagination<TType>(int eventId, int page, int length, Expression<Func<EventAdmins, TType>> selector)
        {
            return FindByCondition(e => e.EventId == eventId)
                .Select(selector)
                .Skip((page - 1) * length)
                .Take(length)
                .ToList();
        }

        public bool IsUserAdminById(int eventId, int userId)
        {
            return ExistsByCondition(e => e.EventId == eventId && e.UserId == userId);
        }
    }
}