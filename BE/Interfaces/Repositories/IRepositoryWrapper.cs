using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Interfaces.Repositories.Chat;
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
        IUserEventsRepository UserEvents { get; }
        IEventRepository Event { get; }
        IEventAdminsRepository EventAdmins { get; }
        IUserPostCommentsRepository UserPostComments { get; }
        IChatRepository Chat { get; }
        IChatMessageRepository ChatMessage { get; }
        IChatMessagesRepository ChatMessages { get; }
        IChatParticipantsRepository ChatParticipants { get; }
        void Save();
    }
}
