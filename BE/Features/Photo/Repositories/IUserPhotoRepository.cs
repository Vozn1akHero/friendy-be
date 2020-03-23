using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.User.Repositories
{
    public interface IUserPhotoRepository : IRepositoryBase<UserImage>
    {
        Task<IEnumerable<UserImage>> GetRangeAsync(int authorId, int startIndex, int
            length);

        Task Add(UserImage userImage);

        Task<IEnumerable<TType>> GetRangeWithPaginationAsync<TType>(int userId,
            int page, int length, Expression<Func<UserImage, TType>> selector);

        void DeleteByEntity(UserImage userImage);
        UserImage GetById(int id);
    }
}