using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.PostDtos;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BE.Features.Post.Services
{
    public interface IUserPostService
    {
        IEnumerable<UserPostDto> GetRangeByUserId(int userId, int startIndex,
            int length);
        IEnumerable<UserPostDto> GetByPage(int userId, int page);
        UserPostDto GetByPostId(int postId, int userId);
        Task<UserPostDto> CreateAndReturnAsync(int authorId, string content,
            IFormFile file);
    }

    public class UserPostService : IUserPostService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public UserPostService(IRepositoryWrapper repository, IMapper mapper,
            IImageSaver imageSaver)
        {
            _repository = repository;
            _mapper = mapper;
            _imageSaver = imageSaver;
        }
        
        public IEnumerable<UserPostDto> GetRangeByUserId(int userId,
            int startIndex, int length)
        {
            var userPosts = _repository.UserPost.GetRangeById(userId, startIndex, length);
            var userPostsDtos = _mapper.Map<IEnumerable<UserPostDto>>(userPosts,
                opt => opt.Items["userId"] = userId);
            return userPostsDtos;
        }

        public IEnumerable<UserPostDto> GetByPage(int userId, int page)
        {
            return _repository.UserPost.GetByPage(userId, page, UserPostDto
                .Selector(userId));
        }


        public UserPostDto GetByPostId(int postId, int userId)
        {
            var post = _repository.UserPost.GetById(postId);
            var userPostDto =
                _mapper.Map<UserPostDto>(post, opt => opt.Items["userId"] = userId);
            return userPostDto;
        }

        public async Task<UserPostDto> CreateAndReturnAsync(int authorId, string content,
            IFormFile file)
        {
            string imagePath = null;
            if (file != null)
            {
                imagePath = await _imageSaver
                    .SaveAndReturnImagePath(file, 
                        "EventPost",
                        authorId);
            }
            var userPost = new UserPost
            {
                Post = new Models.Post
                {
                    Content = content, 
                    ImagePath = imagePath,
                    Date = DateTime.Now
                },
                UserId = authorId
            };
            _repository.UserPost.Add(userPost);
            await _repository.SaveAsync();
            var post = _repository.UserPost.GetById(userPost.Id);
            var userPostDto =
                _mapper.Map<UserPostDto>(post, opt => opt.Items["userId"] = authorId);
            return userPostDto;
        }
    }
}