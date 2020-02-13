using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces
{
    public interface IEventPhotoRepository : IRepositoryBase<EventImage>
    {
        Task<IEnumerable<TType>> SelectWithPaginationAsync<TType>(int eventId,
            int page, Expression<Func<EventImage, TType>> selector);

        Task<IEnumerable<EventImage>> GetRangeAsync(int eventId, int startIndex, int 
        length);
        Task Add(int eventId, int photoId);
    }
}