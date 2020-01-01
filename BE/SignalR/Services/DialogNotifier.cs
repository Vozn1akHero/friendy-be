using System;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BE.SignalR.Services
{
    public interface IDialogNotifierService
    {
        Task SendNewMessageAsync(string groupName, CreatedMessageDto 
        chatMessage);
        Task SendNewExpandedMessageAsync(string groupName, 
        ChatLastMessageDto 
        chatMessage);
    }
    
    public class DialogNotifierService : IDialogNotifierService
    {
        private IHubContext<DialogHub> _dialogHub;
        
        public DialogNotifierService(IHubContext<DialogHub> dialogHub)
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
            await _dialogHub.Clients.Group(groupName).SendAsync("SendExpandedMessageToUser",
                chatMessage);
        }
    }
}

