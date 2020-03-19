using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Comment.Dtos;
using BE.Helpers;
using BE.Helpers.CustomExceptions;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BE.Features.Comment.Services
{
    public interface IPostCommentService
    {
        Task<CommentLike> LikeAsync(int id, int userId);
        Task<CommentLike> UnlikeAsync(int id, int userId);

        Task<IEnumerable<PostCommentDto>>
            GetAllMainByPostIdAsync(int postId,
                int userId);

        Task<PostCommentDto> CreateAndReturnMainCommentAsync(NewCommentDto
            commentDto, int authorId, IFormFile file);
    }

    public class PostCommentService : IPostCommentService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IRepositoryWrapper _repository;

        public PostCommentService(IRepositoryWrapper repository, IImageSaver imageSaver)
        {
            _repository = repository;
            _imageSaver = imageSaver;
        }

        public async Task<CommentLike> LikeAsync(int id, int userId)
        {
            var like = await _repository.CommentLike.FindByCommentIdAndUserId(id, userId);
            if (like != null) throw new EntityIsAlreadyLiked();
            var entity = new CommentLike
            {
                CommentId = id,
                UserId = userId
            };
            _repository.CommentLike.CreateLike(entity);
            await _repository.SaveAsync();
            return entity;
        }

        public async Task<CommentLike> UnlikeAsync(int id, int userId)
        {
            var like = await _repository.CommentLike.FindByCommentIdAndUserId(id, userId);
            if (like != null)
            {
                _repository.CommentLike.Delete(like);
                await _repository.SaveAsync();
            }
            else
            {
                throw new EntityIsNotLikedException();
            }

            return like;
        }

        public async Task<IEnumerable<PostCommentDto>> GetAllMainByPostIdAsync(int postId,
            int userId)
        {
            var posts = await _repository
                .MainComment
                .GetAllByPostIdAsync(postId, PostCommentDto.Selector(userId));
            return posts;
        }

        public async Task<PostCommentDto> CreateAndReturnMainCommentAsync(NewCommentDto
            commentDto, int authorId, IFormFile file)
        {
            string imagePath;
            if (file != null)
                imagePath = await _imageSaver
                    .SaveAndReturnImagePath(file, "Comment",
                        commentDto.PostId);
            var newComment = new MainComment
            {
                Comment = new Models.Comment()
                {
                    PostId = commentDto.PostId,
                    Content = commentDto.Content,
                    Date = DateTime.Now,
                    UserId = authorId
                }
            };
            _repository.MainComment.CreateMainComment(newComment);
            await _repository.SaveAsync();
            var createdComment = _repository.MainComment.FindById(newComment.Id,
                PostCommentDto.Selector(authorId));
            return createdComment;
        }
    }
}