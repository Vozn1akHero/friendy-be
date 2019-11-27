using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.SignalR;

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
            await Clients.All.SendAsync("OnConnectedAsync", "CONNECTED");
            await base.OnConnectedAsync();
        }

        public async Task ConnectUser(int userId)
        {
            
        }
    }
}