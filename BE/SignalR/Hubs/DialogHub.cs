using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
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

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToUser(string group, CreatedMessageDto message)
        {
            await Clients.Group(group).SendAsync("SendMessageToUser", message);
        }
        
        public async Task SendExpandedMessageToUser(string group, ChatLastMessageDto message)
        {
            await Clients.Group(group).SendAsync("SendExpandedMessageToUser", message);
        }
    }
}