using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.CommentDtos;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BE.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task RemovePostCommentsByPostIdAsync(int postId)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddAsync(Comment comment)
        {
           Create(comment);
           await SaveAsync();
        }

        public async Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAsync(int postId, int startIndex, int length)
        {
            var comments = await FindByCondition(e => e.PostId == postId && e.Id >= startIndex)
                .Select(e => new PostCommentDto {
                    Id = e.Id,
                    AuthorId = e.UserId,
                    AuthorName = e.User.Name,
                    AuthorSurname = e.User.Surname,
                    AuthorAvatarPath = e.User.Avatar,
                    Content = e.Content,
                    LikesCount = e.CommentLike.Count,
                    CommentsCount = e.CommentRespond.Count,
                    PostId = e.PostId,
                    Date = e.Date
            }).Take(length).ToListAsync();
            return comments;
        }
        public async Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAuthedAsync(int postId, int startIndex, int length, int userId)
        {
            var comments = await FindByCondition(e => e.PostId == postId && e.Id >= startIndex)
                .Select(e => new PostCommentDto {
                    Id = e.Id,
                    AuthorId = e.UserId,
                    AuthorName = e.User.Name,
                    AuthorSurname = e.User.Surname,
                    AuthorAvatarPath = e.User.Avatar,
                    Content = e.Content,
                    LikesCount = e.CommentLike.Count,
                    CommentsCount = e.CommentRespond.Count,
                    PostId = e.PostId,
                    IsCommentLikedByUser = e.CommentLike.Any(d => d.UserId == userId),
                    Date = e.Date
            }).Take(length).ToListAsync();
            return comments;
        }
        
    }
}