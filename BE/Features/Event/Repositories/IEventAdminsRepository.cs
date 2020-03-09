using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventAdminsRepository : IRepositoryBase<EventAdmins>
    {
        Task DeleteByEventIdAndAdminId(int eventId, int adminId);
        IEnumerable<TType> FilterRangeByEventIdAndKeyword<TType>(int eventId,
            string keyword, int page, int length,
            Expression<Func<EventAdmins, TType>> selector);
        IEnumerable<TType> GetRangeByEventIdWithPagination<TType>(int eventId, int page, int length,
            Expression<Func<EventAdmins, TType>> selector);
        bool IsUserAdminById(int eventId, int userId);
    }
}