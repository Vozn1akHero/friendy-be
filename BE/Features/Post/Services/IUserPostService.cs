using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Post.Dtos;
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
}