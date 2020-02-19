using System.Threading.Tasks;
using BE.Dtos.ChatDtos.ClientDtos;
using BE.Dtos.ChatDtos.ServerDtos;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BE.SignalR.Services
{
    public interface IDialogNotifier
    {
        Task SendNewMessageAsync(string groupName, CreatedMessageDto
            chatMessage);

        Task SendNewExpandedMessageAsync(string groupName,
            ChatLastMessageDto
                chatMessage);
    }

    public class DialogNotifier : IDialogNotifier
    {
        private readonly IHubContext<DialogHub> _dialogHub;

        public DialogNotifier(IHubContext<DialogHub> dialogHub)
        {
            _dialogHub = dialogHub;
        }

        public async Task SendNewMessageAsync(string groupName,
            CreatedMessageDto
                chatMessage)
        {
            await _dialogHub.Clients.Group(groupName).SendAsync("SendMessageToUser",
                chatMessage);
        }

        public async Task SendNewExpandedMessageAsync(string groupName,
            ChatLastMessageDto
                chatMessage)
        {
            await _dialogHub.Clients.Group(groupName).SendAsync(
                "SendExpandedMessageToUser",
                chatMessage);
        }
    }
}