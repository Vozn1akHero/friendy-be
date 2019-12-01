using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.CommentDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task RemovePostCommentsByPostIdAsync(int postId);
        Task AddAsync(Comment comment);
        Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAsync(int postId, int startIndex, int length);
        Task<IEnumerable<PostCommentDto>> GetRangeByPostIdAuthedAsync(int postId, int startIndex, int length, int userId);
    }
}