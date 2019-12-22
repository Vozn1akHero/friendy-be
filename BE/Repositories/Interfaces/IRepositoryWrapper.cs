using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Interfaces.Repositories.Chat;
using BE.Repositories;
using BE.Repositories.Interfaces;
using BE.Repositories.Interfaces.Event;
using BE.Repositories.Interfaces.User;

namespace BE.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IAuthenticationSessionRepository AuthenticationSession { get; }
        IPostRepository Post { get; }
        IUserPostRepository UserPost { get; }
        IUserPhotoRepository UserPhoto { get; }
        IPostLikeRepository PostLike { get; }
        //IFriendRepository Friend { get; }
        IUserFriendshipRepository UserFriendship { get; }
        IUserEventsRepository UserEvents { get; }
        IEventRepository Event { get; }
        IEventAdminsRepository EventAdmins { get; }
        ICommentRepository Comment { get; }
        IChatRepository Chat { get; }
        IChatMessageRepository ChatMessage { get; }
        IChatMessagesRepository ChatMessages { get; }
        //IChatParticipantsRepository ChatParticipants { get; }
        IFriendRequestRepository FriendRequest { get; }
        IEventPostRepository EventPost { get; }
        IEventParticipantsRepository EventParticipants { get; }
        IEventPhotoRepository EventPhoto { get; }
        IPhotoRepository Photo { get; }
        IEventParticipationRequestRepository EventParticipationRequest { get; }
    }
}
