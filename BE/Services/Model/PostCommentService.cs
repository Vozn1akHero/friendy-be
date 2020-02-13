using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Dtos.CommentDtos;
using BE.Helpers.CustomExceptions;
using BE.Interfaces;
using BE.Models;
using Microsoft.Extensions.Logging;

namespace BE.Services.Model
{
    public interface IPostCommentService
    {
        Task<CommentLike> Like(int id, int userId);
        Task<CommentLike> Unlike(int id, int userId);
        Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAsync(int postId,
            int startIndex, int length);
        Task<IEnumerable<PostCommentDto>>
            GetAllMainByPostIdAuthedAsync(int postId,
                int userId);
        Task AddAsync(Comment comment);
    }
    
    public class PostCommentService : IPostCommentService
    {
        private IRepositoryWrapper _repository;

        public PostCommentService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<CommentLike> Like(int id, int userId)
        {
            var like = await _repository.CommentLike.FindByCommentIdAndUserId(id, userId);
            if (like != null)
            {
                throw new EntityIsAlreadyLiked();
            }
            var newLike = new CommentLike
            {
                CommentId = id,
                UserId = userId
            };
            await _repository.Comment.CreateLikeAsync(newLike);
            return newLike;
        }

        public async Task<CommentLike> Unlike(int id, int userId)
        {
            var like = await _repository.CommentLike.FindByCommentIdAndUserId(id, userId);
            if (like != null)
                await _repository.Comment.UnlikeAsync(like);
            else throw new EntityIsNotLikedException();
            return like;
        }

        public Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAsync(int postId, int startIndex, int length)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<PostCommentDto>> 
            GetAllMainByPostIdAuthedAsync(int postId, 
                int userId)
        {
            Expression<Func<MainComment, PostCommentDto>> selectQuery = e =>
                new PostCommentDto
                {
                    Id = e.Comment.Id,
                    AuthorId = e.Comment.UserId,
                    AuthorName = e.Comment.User.Name,
                    AuthorSurname = e.Comment.User.Surname,
                    AuthorAvatarPath = e.Comment.User.Avatar,
                    Content = e.Comment.Content,
                    LikesCount = e.Comment.CommentLike.Count,
                    CommentsCount = e.ResponseToComment.Count,
                    PostId = postId,
                    IsCommentLikedByUser = e.Comment.CommentLike.Any(d => d.UserId == userId),
                    Date = e.Comment.Date
                };
            var posts = await _repository
                .MainComment
                .GetAllByPostIdAsync(postId, selectQuery);
            return posts;
        }

        public async Task<MainCommentResponse> CreateMainComment()
        {
            return null;
        }

        public Task AddAsync(Comment comment)
        {
            throw new System.NotImplementedException();
        }
    }
}