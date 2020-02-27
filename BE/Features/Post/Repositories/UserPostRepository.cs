using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Comment.Repositories;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Post.Repositories
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

        public IEnumerable<UserPost> GetRangeById(int userId,
            int startIndex, int length)
        {
            var posts = FindByCondition(e => e.UserId == userId && e.Id >= startIndex)
                    .Include(e => e.Post)
                    .Include(e => e.Post.PostLike)
                    .Include(e => e.Post.Comment)
                    .Include(e => e.User)
                    .Take(length)
                    .OrderByDescending(e => e.Id)
                    .ToList();
            return posts;
        }

        public IEnumerable<T> GetByPage<T>(int userId,
            int page,
            Expression<Func<UserPost, T>> selector)
        {
            int length = 20;
            return FindByCondition(e => e.UserId == userId)
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * length)
                .Take(length)
                .Select(selector)
                .ToList();
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