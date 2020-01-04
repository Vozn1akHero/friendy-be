using System;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BE.SignalR.Hubs
{
    public class UserStatusHub : Hub
    {
        private IRepositoryWrapper _repository;

        public UserStatusHub(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var session =
                await _repository.Session.GetByConnectionId(Context.ConnectionId);
            session.ConnectionEnd = DateTime.Now;
            await _repository.Session.UpdateAndReturn(session);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ConnectUser(int userId)
        {
            var session = await _repository.Session.GetByUserId(userId);
            if (session != null)
            {
                session.ConnectionEnd = null;
                session.ConnectionStart = DateTime.Now;
                session.ConnectionId = Context.ConnectionId;
                await _repository.Session.UpdateAndReturn(session);
            }
            else
            {
                await _repository.Session.CreateAndReturn(new Session
                {
                    UserId = userId,
                    ConnectionStart = DateTime.Now,
                    ConnectionId = Context.ConnectionId
                });
            }
        }
    }
}