using System;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace BE.SignalR.Hubs
{
    public class UserStatusHub : Hub
    {
        //private readonly IRepositoryWrapper _repository;
        private FriendyContext _friendyContext;

        public UserStatusHub(IRepositoryWrapper repository, FriendyContext friendyContext)
        {
            //_repository = repository;
            _friendyContext = friendyContext;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var session =
                _friendyContext.Session.SingleOrDefault(e =>
                    e.ConnectionId == Context.ConnectionId);
            if (session != null)
            {
                session.ConnectionEnd = DateTime.Now;
                //await _repository.Session.UpdateAndReturn(session);
                await _friendyContext.SaveChangesAsync();
                await base.OnDisconnectedAsync(exception);
            }
        }

        public async Task ConnectUser(int userId)
        {
            var user = _friendyContext.User.SingleOrDefault(e => e.Id == userId);
            if (user != null)
            {
                if (user.Session != null)
                {
                    user.Session.ConnectionEnd = null;
                    user.Session.ConnectionStart = DateTime.Now;
                    user.Session.ConnectionId = Context.ConnectionId;
                    //await _repository.Session.UpdateAndReturn(session);
                }
                else
                {
                    user.Session = new Session
                    {
                        ConnectionStart = DateTime.Now,
                        ConnectionId = Context.ConnectionId
                    };
                }

                await _friendyContext.SaveChangesAsync();
            }
        }
    }
}