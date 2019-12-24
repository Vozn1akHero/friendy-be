using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;

namespace BE.Services.Model
{
    public interface IUserPostService
    {
        Task<IEnumerable<UserPostDto>> GetRangeByUserIdAsync(int userId, int startIndex, int length);
        Task<UserPostDto> GetByPostId(int postId, int userId);
        Task<UserPostDto> CreateAndReturnAsync(UserPost userPost);
    }
    
    public class UserPostService : IUserPostService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        
        public UserPostService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<UserPostDto>> GetRangeByUserIdAsync(int userId, int startIndex, int length)
        {
            var userPosts = await _repository.UserPost.GetRangeByIdAsync(userId, startIndex, length);
            var userPostsDtos = _mapper.Map<IEnumerable<UserPostDto>>(userPosts, opt => opt.Items["userId"] = userId);
            return userPostsDtos;
        }

        public async Task<UserPostDto> CreateAndReturnAsync(UserPost userPost)
        {
            var createdUserPost = await _repository.UserPost.CreateAndReturnAsync(userPost);
            var post = await _repository.UserPost.GetByIdAsync(createdUserPost.Id);
            var userPostDto = _mapper.Map<UserPostDto>(post);
            return userPostDto;
        }

        public async Task<UserPostDto> GetByPostId(int postId, int userId)
        {
            var post = await _repository.UserPost.GetByPostIdAsync(postId);
            var userPostDto = _mapper.Map<UserPostDto>(post, opt => opt.Items["userId"] = userId);
            return userPostDto;
        }
    }
}