using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Helpers.CustomExceptions;
using BE.Interfaces;
using BE.Models;

namespace BE.Services.Model
{
    public interface ICommentResponseService
    {
        Task<CommentResponseLike> Like(int id, int userId);
        Task Unlike(int id, int userId);
        Task<IEnumerable<PostCommentResponseDto>>
            GetAllCommentResponsesAsync(int commentId,
                int userId);
    }
    
    public class CommentResponseService : ICommentResponseService
    {
        private IRepositoryWrapper _repository;

        public CommentResponseService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<CommentResponseLike> Like(int id, int userId)
        {
            try
            {
                var like = new CommentResponseLike
                {
                    CommentResponseId = id,
                    UserId = userId
                };
                await _repository.Comment.LikeResponseAsync(like);
                return like;
            }
            catch (EntityIsAlreadyLiked e)
            {
                throw;
            }
        }

        public async Task Unlike(int id, int userId)
        {
            try
            {
                await _repository.Comment.UnlikeResponseByResponseIdAndUserIdAsync(id, userId);
            }
            catch (EntityIsAlreadyLiked e)
            {
                throw;
            }
        }
        
        public async Task<IEnumerable<PostCommentResponseDto>> 
            GetAllCommentResponsesAsync(int commentId,
                int userId)
        {
            var posts = await _repository.ResponseToComment
                .GetAllByMainCommentIdAsync(commentId, 
                    PostCommentResponseDto.Selector(userId));
            return posts;
        }
    }
}