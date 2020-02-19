using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Features.Comment.Repositories;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Post
{
    public class UserPostRepository : RepositoryBase<UserPost>, IUserPostRepository
    {
        public UserPostRepository(FriendyContext friendyContext)
            : base(friendyContext)
        {
        }

        public async Task RemoveByIdAsync(int id)
        {
            var post = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(post);
            await SaveAsync();
        }

        public async Task<IEnumerable<UserPost>> GetRangeByIdAsync(int userId,
            int startIndex, int length)
        {
            var posts =
                await FindByCondition(e => e.UserId == userId && e.Id < startIndex)
                    .Include(e => e.Post)
                    .Include(e => e.Post.PostLike)
                    .Include(e => e.Post.Comment)
                    .Include(e => e.User)
                    .Take(length)
                    .OrderByDescending(e => e.Id)
                    .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<UserPost>> GetRangeByMinDateAsync(int userId,
            DateTime date, int length)
        {
            var posts =
                await FindByCondition(e => e.UserId == userId && e.Post.Date >= date)
                    .Include(e => e.Post)
                    .Include(e => e.Post.PostLike)
                    .Include(e => e.Post.Comment)
                    .Include(e => e.User)
                    .Take(length)
                    .OrderByDescending(e => e.Post.Date)
                    .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<UserPost>> GetLastByUserIdAsync(int userId,
            int length)
        {
            var posts = await FindByCondition(e => e.UserId == userId)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .Include(e => e.User)
                .Take(length)
                .OrderByDescending(e => e.Id)
                .ToListAsync();
            return posts;
        }

        public void Add(UserPost post)
        {
            Create(post);
        }

        public UserPost GetById(int id)
        {
            return FindByCondition(e => e.Id == id)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .Include(e => e.User)
                .SingleOrDefault();
        }

        public async Task<UserPost> GetByIdAsync(int postId)
        {
            return await FindByCondition(e => e.PostId == postId)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .Include(e => e.User)
                .SingleOrDefaultAsync();
        }
    }
}