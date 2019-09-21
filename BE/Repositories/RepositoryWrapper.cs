using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Entities;
using BE.Helpers;
using BE.Interfaces;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BE.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private FriendyContext _friendyContext;
        
        private IUserRepository _user;
        private ISessionRepository _session;
        private IUserPostRepository _userPost;
        private IUserPostLikesRepository _userPostLikes;
        private IFriendRepository _friend;
        private IEventRepository _event;
        private IUserFriendsRepository _userFriends;
        private IUserEventsRepository _userEvents;

        private IJwtService _jwtService;

        public IUserRepository User
        {
            get { return _user ?? (_user = new UserRepository(_friendyContext)); }
        }

        public ISessionRepository Session
        {
            get { return _session ?? (_session = new SessionRepository(_friendyContext)); }
        }
        
        public IUserPostRepository UserPost
        {
            get { return _userPost ?? (_userPost = new UserPostRepository(_friendyContext)); }
        }
         
        public IUserPostLikesRepository UserPostLikes
        {
            get { return _userPostLikes ?? (_userPostLikes = new UserPostLikesRepository(_friendyContext)); }
        }    
        
        public IFriendRepository Friend
        {
            get { return _friend ?? (_friend = new FriendRepository(_friendyContext)); }
        }  
        
        public IEventRepository Event
        {
            get { return _event ?? (_event = new EventRepository(_friendyContext)); }
        }
        
        public IUserFriendsRepository UserFriends
        {
            get { return _userFriends ?? (_userFriends = new UserFriendsRepository(_friendyContext)); }
        }
        
        public IUserEventsRepository UserEvents
        {
            get { return _userEvents ?? (_userEvents = new UserEventsRepository(_friendyContext)); }
        }
        
        public RepositoryWrapper(FriendyContext friendyContext, IJwtService jwtService)
        {
            _friendyContext = friendyContext;
            _jwtService = jwtService;
        }

        public void Save()
        {
            _friendyContext.SaveChanges();
        }
    }
}
