using System;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BE.SignalR.Services
{
    public interface IDialogNotifier
    {
        Task NewMessageNotifierAsync(string groupName, CreatedMessageDto chatMessage);
        Task NewMessageExpandedNotifierAsync(string groupName, ChatLastMessageDto chatMessage);
    }
    
    public class DialogNotifier : IDialogNotifier
    {
        private IRepositoryWrapper _repository;
        private IHubContext<DialogHub> _dialogHub;
        
        public DialogNotifier(IRepositoryWrapper repository, 
            IHubContext<DialogHub> dialogHub)
        {
            _repository = repository;
            _dialogHub = dialogHub;
        }
        
        public async Task NewMessageNotifierAsync(string groupName, CreatedMessageDto chatMessage)
        {
            await _dialogHub.Clients.Group(groupName).SendAsync("SendMessageToUser", chatMessage);
        }

        public async Task NewMessageExpandedNotifierAsync(string groupName, ChatLastMessageDto chatMessage)
        {
            await _dialogHub.Clients.Group(groupName).SendAsync("SendExpandedMessageToUser", chatMessage);
        }
    }
}