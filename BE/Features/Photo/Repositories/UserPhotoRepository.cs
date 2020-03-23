using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.User.Repositories;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Photo
{
    public class UserPhotoRepository : RepositoryBase<UserImage>, IUserPhotoRepository
    {
        public UserPhotoRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<IEnumerable<TType>> GetRangeWithPaginationAsync<TType>(int userId,
            int page, int length, Expression<Func<UserImage, TType>> selector)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Skip((page - 1) * length)
                .Take(length)
                .OrderByDescending(e => e.Id)
                .Select(selector)
                .ToListAsync();
        }

        public void DeleteByEntity(UserImage userImage)
        {
            Delete(userImage);
        }

        public UserImage GetById(int id)
        {
            return FindByCondition(e => e.Id == id)
                .SingleOrDefault();
        }

        public async Task<IEnumerable<UserImage>> GetRangeAsync(int authorId, int
            startIndex, int length)
        {
            return await FindByCondition(e => e.UserId == authorId && e.Id >= startIndex)
                .Take(length)
                .ToListAsync();
        }

        public async Task Add(UserImage userImage)
        {
            Create(userImage);
            await SaveAsync();
        }
    }
}