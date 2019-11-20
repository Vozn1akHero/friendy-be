using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Interfaces.Repositories.Chat;
using BE.Repositories;
using BE.Repositories.Interfaces;

namespace BE.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ISessionRepository Session { get; }
        IPostRepository Post { get; }
        IUserPostRepository UserPost { get; }
        IPostLikeRepository PostLike { get; }
        IFriendRepository Friend { get; }
        IUserFriendsRepository UserFriends { get; }
        IUserEventsRepository UserEvents { get; }
        IEventRepository Event { get; }
        IEventAdminsRepository EventAdmins { get; }
        ICommentRepository Comment { get; }
        IChatRepository Chat { get; }
        IChatMessageRepository ChatMessage { get; }
        IChatMessagesRepository ChatMessages { get; }
        IChatParticipantsRepository ChatParticipants { get; }
        IFriendRequestRepository FriendRequest { get; }
        IEventPostRepository EventPost { get; }
        void Save();
    }
}
