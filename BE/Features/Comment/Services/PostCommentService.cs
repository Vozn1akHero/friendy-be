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
        Task<CommentLike> Like(int id, int userId);
        Task<CommentLike> Unlike(int id, int userId);

        Task<IEnumerable<PostCommentDto>>
            GetAllMainByPostIdAsync(int postId,
                int userId);

        Task<PostCommentDto> CreateAndReturnMainComment(NewCommentDto
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

        public async Task<CommentLike> Like(int id, int userId)
        {
            var like = await _repository.CommentLike.FindByCommentIdAndUserId(id, userId);
            if (like != null) throw new EntityIsAlreadyLiked();
            var newLike = new CommentLike
            {
                CommentId = id,
                UserId = userId
            };
            await _repository.Comment.CreateLikeAsync(newLike);
            await _repository.SaveAsync();
            return newLike;
        }

        public async Task<CommentLike> Unlike(int id, int userId)
        {
            var like = await _repository.CommentLike.FindByCommentIdAndUserId(id, userId);
            if (like != null)
            {
                await _repository.Comment.UnlikeAsync(like);
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

        public async Task<PostCommentDto> CreateAndReturnMainComment(NewCommentDto
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
            _repository.MainComment.Insert(newComment);
            await _repository.MainComment.SaveAsync();
            var createdComment = await _repository.MainComment.FindById(newComment.Id,
                PostCommentDto.Selector(authorId));
            return createdComment;
        }

        public Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAsync(int postId,
            int startIndex, int length)
        {
            throw new NotImplementedException();
        }
    }
}