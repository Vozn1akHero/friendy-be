using System;
using System.Threading.Tasks;
using BE.Features.Chat.Repositories;
using BE.Features.Comment.Repositories;
using BE.Features.Event.Repositories;
using BE.Features.Friendship.Repositories;
using BE.Features.FriendshipRecommendation;
using BE.Features.Photo;
using BE.Features.Photo.Repositories;
using BE.Features.Post.Repositories;
using BE.Features.User.Repositories;
using BE.Models;

namespace BE.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly FriendyContext _friendyContext;
        private IChatRepository _chat;
        private IChatMessageRepository _chatMessage;
        private IChatMessagesRepository _chatMessages;
        private ICommentRepository _comment;

        private ICommentLikeRepository _commentLike;

        //private IFriendRepository _friend;
        private IEventRepository _event;
        private IEventAdminsRepository _eventAdmins;
        private IEventBannedUsersRepository _eventBannedUsers;
        private IEventParticipantsRepository _eventParticipants;
        private IEventParticipationRequestRepository _eventParticipationRequest;
        private IEventPhotoRepository _eventPhoto;

        private IEventPostRepository _eventPost;

        //private IChatParticipantsRepository _chatParticipants;
        private IFriendRequestRepository _friendRequest;
        private IFriendshipRecommendationRepository _friendshipRecommendation;
        private IMainCommentRepository _mainComment;
        private IPhotoRepository _photo;
        private IPostRepository _post;
        private IPostLikeRepository _postLike;
        private IResponseToCommentRepository _responseToComment;
        private ISessionRepository _session;

        private IUserRepository _user;
        private IUserEventsRepository _userEvents;
        private IUserFriendshipRepository _userFriendship;
        private IUserPhotoRepository _userPhoto;
        private IUserPostRepository _userPost;


        /*public IChatParticipantsRepository ChatParticipants =>
            _chatParticipants ?? (_chatParticipants = new ChatParticipantsRepository(_friendyContext, 
                _avatarConverter, 
                _customSqlQuery));*/


        public RepositoryWrapper() : this(new FriendyContext())
        {
        }

        public RepositoryWrapper(FriendyContext friendyContext)
        {
            _friendyContext = friendyContext;
        }


        public virtual IMainCommentRepository MainComment =>
            _mainComment ?? (_mainComment = new MainCommentRepository
                (_friendyContext));

        public virtual IResponseToCommentRepository ResponseToComment =>
            _responseToComment ?? (_responseToComment = new ResponseToCommentRepository
                (_friendyContext));

        public async Task SaveAsync()
        {
            await _friendyContext.SaveChangesAsync();
        }

        public virtual IUserRepository User =>
            _user ?? (_user = new UserRepository(_friendyContext));

        public virtual IUserPhotoRepository UserPhoto => _userPhoto ?? (_userPhoto =
                                                     new UserPhotoRepository(
                                                         _friendyContext));

        public virtual IPostRepository Post =>
            _post ?? (_post = new PostRepository(_friendyContext));

        public virtual IUserPostRepository UserPost =>
            _userPost ?? (_userPost = new UserPostRepository(_friendyContext));

        public virtual IPostLikeRepository PostLike =>
            _postLike ?? (_postLike = new PostLikeRepository(_friendyContext));

        //public IFriendRepository Friend => _friend ?? (_friend = new FriendRepository(_friendyContext));
        public virtual IEventRepository Event =>
            _event ?? (_event = new EventRepository(_friendyContext));

        public virtual IUserFriendshipRepository UserFriendship =>
            _userFriendship ?? (_userFriendship =
                new UserFriendshipRepository(_friendyContext));

        public virtual IUserEventsRepository UserEvents =>
            _userEvents ??
            (_userEvents = new UserEventsRepository(_friendyContext));

        public virtual IEventAdminsRepository EventAdmins =>
            _eventAdmins ?? (_eventAdmins =
                new EventAdminsRepository(_friendyContext));

        public virtual ICommentRepository Comment =>
            _comment ?? (_comment = new CommentRepository(_friendyContext));

        public virtual IChatRepository Chat =>
            _chat ?? (_chat =
                new ChatRepository(_friendyContext));

        public virtual IChatMessageRepository ChatMessage =>
            _chatMessage ?? (_chatMessage =
                new ChatMessageRepository(_friendyContext));

        public virtual IChatMessagesRepository ChatMessages =>
            _chatMessages ?? (_chatMessages =
                new ChatMessagesRepository(_friendyContext));

        public virtual IFriendRequestRepository FriendRequest =>
            _friendRequest ?? (_friendRequest =
                new FriendRequestRepository(_friendyContext));

        public virtual IEventPostRepository EventPost =>
            _eventPost ??
            (_eventPost = new EventPostRepository(_friendyContext));

        public virtual IEventParticipantsRepository EventParticipants =>
            _eventParticipants
            ?? (_eventParticipants =
                new EventParticipantsRepository(_friendyContext));

        public virtual IEventPhotoRepository EventPhoto =>
            _eventPhoto ??
            (_eventPhoto = new EventPhotoRepository(_friendyContext));

        public virtual IPhotoRepository Photo =>
            _photo ?? (_photo = new PhotoRepository(_friendyContext));

        public virtual IEventParticipationRequestRepository EventParticipationRequest =>
            _eventParticipationRequest ?? (_eventParticipationRequest =
                new EventParticipationRequestRepository(_friendyContext));

        public virtual IFriendshipRecommendationRepository FriendshipRecommendation =>
            _friendshipRecommendation ?? (_friendshipRecommendation =
                new FriendshipRecommendationRepository(_friendyContext));

        public virtual ISessionRepository Session => _session ?? (_session =
                                                 new SessionRepository(_friendyContext));

        public virtual IEventBannedUsersRepository EventBannedUsers =>
            _eventBannedUsers ?? (_eventBannedUsers =
                new EventBannedUsersRepository(_friendyContext));

        public virtual ICommentLikeRepository CommentLike => _commentLike ??
                                                     (_commentLike =
                                                         new CommentLikeRepository(
                                                             _friendyContext));

        public int Save()
        {
            return _friendyContext.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}