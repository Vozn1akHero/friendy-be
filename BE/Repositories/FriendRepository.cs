using System.Collections.Generic;
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


    }
}