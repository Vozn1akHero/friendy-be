using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Comment.Dto;
using BE.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Http;
using BE.Models;
namespace BE.Features.Comment.Services
{
    public interface ICommentResponseService
    {
        Task<CommentResponseLike> Like(int id, int userId);
        Task Unlike(int id, int userId);

        Task<IEnumerable<PostCommentResponseDto>>
            GetAllCommentResponsesAsync(int commentId,
                int userId);

        Task<PostCommentResponseDto> CreateAndReturnDtoAsync(
            TransferredResponseToResponseDto
                responseDto, int authorId, IFormFile file);

        Task<PostCommentResponseDto> CreateAndReturnDtoAsync(
            NewCommentResponseDto commentResponseDto, int authorId,
            IFormFile file);
    }

    public class CommentResponseService : ICommentResponseService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IRepositoryWrapper _repository;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public CommentResponseService(IRepositoryWrapper repository,
            IImageSaver imageSaver)
        {
            _repository = repository;
            _imageSaver = imageSaver;
        }

        public async Task<CommentResponseLike> Like(int id, int userId)
        {
            var like = new CommentResponseLike
            {
                CommentResponseId = id,
                UserId = userId
            };
            await _repository.Comment.LikeResponseAsync(like);
            await _repository.SaveAsync();
            return like;
        }

        public async Task Unlike(int id, int userId)
        {
            await _repository.Comment
                .UnlikeResponseByResponseIdAndUserIdAsync(id, userId);
            await _repository.SaveAsync();
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

        public async Task<PostCommentResponseDto> CreateAndReturnDtoAsync
            (TransferredResponseToResponseDto responseDto, int authorId, IFormFile file)
        {
            string imagePath;
            if (file != null)
                imagePath = await _imageSaver
                    .SaveAndReturnImagePath(file, "ResponseToComment",
                        responseDto.ResponseToCommentId);
            var responseToResponse = new ResponseToComment
            {
                Comment = new Models.Comment
                {
                    Content = responseDto.Content,
                    UserId = authorId,
                    PostId = responseDto.PostId,
                    Date = DateTime.Now
                },
                ResponseToCommentId = responseDto.ResponseToCommentId,
                MainCommentId = responseDto.MainCommentId
            };
            await _unitOfWork.ResponseToCommentRepository
                .CreateAsync(responseToResponse);
            _unitOfWork.SaveChanges();
            return await _repository.ResponseToComment.GetByIdAsync(responseToResponse.Id,
                PostCommentResponseDto.Selector(authorId));
        }

        public async Task<PostCommentResponseDto> CreateAndReturnDtoAsync(
            NewCommentResponseDto commentResponseDto, int authorId,
            IFormFile file)
        {
            string imagePath;
            if (file != null)
                imagePath = await _imageSaver
                    .SaveAndReturnImagePath(file, "ResponseToCommentResponse",
                        commentResponseDto.CommentId);
            var responseToResponse = new ResponseToComment
            {
                Comment = new Models.Comment
                {
                    Content = commentResponseDto.Content,
                    UserId = authorId,
                    PostId = commentResponseDto.PostId,
                    Date = DateTime.Now
                },
                ResponseToCommentId = null,
                MainCommentId = commentResponseDto.CommentId
            };
            await _repository.ResponseToComment.CreateAsync(responseToResponse);
            await _repository.ResponseToComment.SaveAsync();
            return await _repository.ResponseToComment.GetByIdAsync(responseToResponse.Id,
                PostCommentResponseDto.Selector(authorId));
        }
    }
}