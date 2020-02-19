using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BE.SignalR.Hubs
{
    public class ProfileHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public Task SendNewPostToUsers(string group, string message)
        {
            return Clients.Group(group).SendAsync("SendNewPostToUsers", message);
        }
    }
}