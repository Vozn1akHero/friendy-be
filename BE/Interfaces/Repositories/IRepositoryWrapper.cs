using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Repositories;

namespace BE.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ISessionRepository Session { get; }
        IUserPostRepository UserPost { get; }
        IUserPostLikesRepository UserPostLikes { get; }
        IFriendRepository Friend { get; }
        IUserFriendsRepository UserFriends { get; }
        void Save();
    }
}
