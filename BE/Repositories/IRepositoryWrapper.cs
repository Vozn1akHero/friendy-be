using System;
using System.Threading.Tasks;
using BE.Features.Chat.Repositories;
using BE.Features.Comment.Repositories;
using BE.Features.Event.Repositories;
using BE.Features.Friendship.Repositories;
using BE.Features.FriendshipRecommendation;
using BE.Features.Photo.Repositories;
using BE.Features.Post.Repositories;
using BE.Features.User.Repositories;

namespace BE.Repositories
{
    public interface IRepositoryWrapper : IDisposable
    {
        IUserRepository User { get; }
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

        IMainCommentRepository MainComment { get; }
        IResponseToCommentRepository ResponseToComment { get; }
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
        IFriendshipRecommendationRepository FriendshipRecommendation { get; }
        ISessionRepository Session { get; }
        IEventBannedUsersRepository EventBannedUsers { get; }
        ICommentLikeRepository CommentLike { get; }
        int Save();
        Task SaveAsync();
    }
}