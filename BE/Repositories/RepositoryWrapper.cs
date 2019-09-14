using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Entities;
using BE.Helpers;
using BE.Interfaces;
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
        private IUserEntryRepository _userEntry;
        private IEntryRepository _entry;

        private IJwtService _jwtService;

        public IUserRepository User
        {
            get { return _user ?? (_user = new UserRepository(_friendyContext)); }
        }

        public ISessionRepository Session
        {
            get { return _session ?? (_session = new SessionRepository(_friendyContext)); }
        }
        
        public IUserEntryRepository UserEntry
        {
            get { return _userEntry ?? (_userEntry = new UserEntryRepository(_friendyContext)); }
        }

        public IEntryRepository Entry
        {
            get { return _entry ?? (_entry = new EntryRepository(_friendyContext)); }
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
