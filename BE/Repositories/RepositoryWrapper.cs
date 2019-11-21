using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Helpers;
using BE.Interfaces;
using BE.Interfaces.Repositories;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using BE.Repositories.Chat;
using BE.Repositories.Event;
using BE.Repositories.Interfaces;
using BE.Repositories.Interfaces.Event;
using BE.Repositories.RepositoryServices.Interfaces.User;
using BE.RepositoryServices.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BE.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private FriendyContext _friendyContext;

        private IUserRepository _user;
        private ISessionRepository _session;
        private IPostRepository _post;
        private IUserPostRepository _userPost;
        private IPostLikeRepository _postLike;
        private IFriendRepository _friend;
        private IEventRepository _event;
        private IUserFriendsRepository _userFriends;
        private IUserEventsRepository _userEvents;
        private IEventAdminsRepository _eventAdmins;
        private ICommentRepository _comment;
        private IChatRepository _chat;
        private IChatMessageRepository _chatMessage;
        private IChatMessagesRepository _chatMessages;
        private IChatParticipantsRepository _chatParticipants;
        private IFriendRequestRepository _friendRequest;
        private IEventPostRepository _eventPost;
        private IEventParticipantsRepository _eventParticipants;

        private IJwtService _jwtService;
        private IAvatarConverterService _avatarConverterService;
        private ICustomSqlQueryService _customSqlQueryService;
        
        private IUserSearchingService _userSearchingService;
        
        public IUserRepository User =>
            _user ?? (_user = new UserRepository(_friendyContext,
                _avatarConverterService,
                _userSearchingService));

        public ISessionRepository Session => _session ?? (_session = new SessionRepository(_friendyContext));
        
        public IPostRepository Post => _post ?? (_post = new PostRepository(_friendyContext));
        
        public IUserPostRepository UserPost => _userPost ?? (_userPost = new UserPostRepository(_friendyContext));

        public IPostLikeRepository PostLike => _postLike ?? (_postLike = new PostLikeRepository(_friendyContext));

        public IFriendRepository Friend => _friend ?? (_friend = new FriendRepository(_friendyContext));

        public IEventRepository Event => _event ?? (_event = new EventRepository(_friendyContext, _avatarConverterService));

        public IUserFriendsRepository UserFriends => _userFriends ?? (_userFriends = new UserFriendsRepository(_friendyContext, _avatarConverterService));

        public IUserEventsRepository UserEvents => _userEvents ?? (_userEvents = new UserEventsRepository(_friendyContext));

        public IEventAdminsRepository EventAdmins => _eventAdmins ?? (_eventAdmins = new EventAdminsRepository(_friendyContext));

        public ICommentRepository Comment => _comment ?? (_comment = new CommentRepository(_friendyContext));

        public IChatRepository Chat => _chat ?? (_chat = new ChatRepository(_friendyContext));

        public IChatMessageRepository ChatMessage => _chatMessage ?? (_chatMessage = new ChatMessageRepository(_friendyContext));

        public IChatMessagesRepository ChatMessages => _chatMessages ?? (_chatMessages = new ChatMessagesRepository(_friendyContext, _avatarConverterService));

        public IFriendRequestRepository FriendRequest => _friendRequest ?? (_friendRequest = new FriendRequestRepository(_friendyContext));
        public IEventPostRepository EventPost => _eventPost ?? (_eventPost = new EventPostRepository(_friendyContext));
        public IEventParticipantsRepository EventParticipants => _eventParticipants ?? (_eventParticipants = new EventParticipantRepository(_friendyContext));

        public IChatParticipantsRepository ChatParticipants =>
            _chatParticipants ?? (_chatParticipants = new ChatParticipantsRepository(_friendyContext, 
                _avatarConverterService, 
                _customSqlQueryService));


        public RepositoryWrapper(FriendyContext friendyContext,
            IJwtService jwtService, 
            IAvatarConverterService avatarConverterService,
            ICustomSqlQueryService customSqlQueryService,
            IUserSearchingService userSearchingService)
        {
            _friendyContext = friendyContext;
            _jwtService = jwtService;
            _avatarConverterService = avatarConverterService;
            _customSqlQueryService = customSqlQueryService;
            _userSearchingService = userSearchingService;
        }

        public void Save()
        {
            _friendyContext.SaveChanges();
        }
    }
}
