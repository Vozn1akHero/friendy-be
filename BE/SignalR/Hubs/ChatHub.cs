using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BE.SignalR.Hubs
{
    public class DialogHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("OnConnectedAsync", "CONNECTED");
            await base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("SendMessageAsync", message);
        }

        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task SendMessageToUser(string group, string message)
        {
            return Clients.Group(group).SendAsync("SendMessageToUser", message);
        }
    }
}