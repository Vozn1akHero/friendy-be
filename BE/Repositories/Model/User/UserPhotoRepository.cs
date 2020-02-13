using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.PhotoDtos;
using BE.Models;
using BE.Repositories.Interfaces.User;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.User
{
    public class UserPhotoRepository : RepositoryBase<UserImage>, IUserPhotoRepository
    {
        public UserPhotoRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<IEnumerable<UserImage>> GetRangeWithPaginationAsync(
            int userId, int page)
        {
            int length = 25;
            return await FindByCondition(e => e.UserId == userId)
                .Skip((page-1)*length)
                .Take(length)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<UserImage>> GetRangeAsync(int authorId, int 
        startIndex, int length)
        {
            return await FindByCondition(e => e.UserId == authorId && e.Id >= startIndex)
                .Take(length)
                .ToListAsync();
        }

        public async Task Add(UserImage userImage)
        {
            Create(userImage);
            await SaveAsync();
        }

    }
}