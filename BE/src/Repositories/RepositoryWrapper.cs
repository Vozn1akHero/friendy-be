using System.Threading.Tasks;
using BE.Features.Chat.Repositories;
using BE.Features.Comment.Repositories;
using BE.Features.Event.Repositories;
using BE.Features.Friendship.Repositories;
using BE.Features.FriendshipRecommendation;
using BE.Features.Photo;
using BE.Features.Photo.Repositories;
using BE.Features.Post;
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


        public IMainCommentRepository MainComment =>
            _mainComment ?? (_mainComment = new MainCommentRepository
                (_friendyContext));

        public IResponseToCommentRepository ResponseToComment =>
            _responseToComment ?? (_responseToComment = new ResponseToCommentRepository
                (_friendyContext));

        public async Task SaveAsync()
        {
            await _friendyContext.SaveChangesAsync();
        }

        public IUserRepository User =>
            _user ?? (_user = new UserRepository(_friendyContext));

        public IUserPhotoRepository UserPhoto => _userPhoto ?? (_userPhoto =
                                                     new UserPhotoRepository(
                                                         _friendyContext));

        public IPostRepository Post =>
            _post ?? (_post = new PostRepository(_friendyContext));

        public IUserPostRepository UserPost =>
            _userPost ?? (_userPost = new UserPostRepository(_friendyContext));

        public IPostLikeRepository PostLike =>
            _postLike ?? (_postLike = new PostLikeRepository(_friendyContext));

        //public IFriendRepository Friend => _friend ?? (_friend = new FriendRepository(_friendyContext));
        public IEventRepository Event =>
            _event ?? (_event = new EventRepository(_friendyContext));

        public IUserFriendshipRepository UserFriendship =>
            _userFriendship ?? (_userFriendship =
                new UserFriendshipRepository(_friendyContext));

        public IUserEventsRepository UserEvents =>
            _userEvents ??
            (_userEvents = new UserEventsRepository(_friendyContext));

        public IEventAdminsRepository EventAdmins =>
            _eventAdmins ?? (_eventAdmins =
                new EventAdminsRepository(_friendyContext));

        public ICommentRepository Comment =>
            _comment ?? (_comment = new CommentRepository(_friendyContext));

        public IChatRepository Chat =>
            _chat ?? (_chat =
                new ChatRepository(_friendyContext));

        public IChatMessageRepository ChatMessage =>
            _chatMessage ?? (_chatMessage =
                new ChatMessageRepository(_friendyContext));

        public IChatMessagesRepository ChatMessages =>
            _chatMessages ?? (_chatMessages =
                new ChatMessagesRepository(_friendyContext));

        public IFriendRequestRepository FriendRequest =>
            _friendRequest ?? (_friendRequest =
                new FriendRequestRepository(_friendyContext));

        public IEventPostRepository EventPost =>
            _eventPost ??
            (_eventPost = new EventPostRepository(_friendyContext));

        public IEventParticipantsRepository EventParticipants =>
            _eventParticipants
            ?? (_eventParticipants =
                new EventParticipantsRepository(_friendyContext));

        public IEventPhotoRepository EventPhoto =>
            _eventPhoto ??
            (_eventPhoto = new EventPhotoRepository(_friendyContext));

        public IPhotoRepository Photo =>
            _photo ?? (_photo = new PhotoRepository(_friendyContext));

        public IEventParticipationRequestRepository EventParticipationRequest =>
            _eventParticipationRequest ?? (_eventParticipationRequest =
                new EventParticipationRequestRepository(_friendyContext));

        public IFriendshipRecommendationRepository FriendshipRecommendation =>
            _friendshipRecommendation ?? (_friendshipRecommendation =
                new FriendshipRecommendationRepository(_friendyContext));

        public ISessionRepository Session => _session ?? (_session =
                                                 new SessionRepository(_friendyContext));

        public IEventBannedUsersRepository EventBannedUsers =>
            _eventBannedUsers ?? (_eventBannedUsers =
                new EventBannedUsersRepository(_friendyContext));

        public ICommentLikeRepository CommentLike => _commentLike ??
                                                     (_commentLike =
                                                         new CommentLikeRepository(
                                                             _friendyContext));

        public IDatabaseTransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(_friendyContext);
        }

        public int Save()
        {
            return _friendyContext.SaveChanges();
        }

        public void Dispose()
        {
            _friendyContext.Dispose();
        }
    }
}