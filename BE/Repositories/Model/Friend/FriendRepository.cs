using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class FriendRepository : RepositoryBase<Friend>, IFriendRepository
    {
        public FriendRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<Friend> GetByUserId(int userId)
        {
            return await FindByCondition(e => e.FriendId == userId)
                .SingleOrDefaultAsync();
        }
    }
}