using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Chat.Dtos;
using BE.Features.Chat.Helpers;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BE.Features.Chat.Services
{
    public interface IChatService
    {
        ChatLastMessageDto GetLastChatMessageByChatId(int chatId, int receiverId);

        IEnumerable<ChatMessageDto> GetMessageByReceiverIdWithPagination(
            int receiverId, int issuerId, int page, int length);

        IEnumerable<ChatLastMessageDto> GetLastMessageByReceiverIdWithPagination(
            int receiverId, int page, int length);

        Task<ChatMessage> CreateAndReturnMessageAsync(string text, IFormFile image,
            int chatId, int authorId, int receiverId);

        Task<InterlocutorsDto> GetByInterlocutorsIdentifiers(int firstParticipantId,
            int secondParticipantId);
    }

    public class ChatService : IChatService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IRepositoryWrapper _repository;

        public ChatService(IRepositoryWrapper repository,
            IImageSaver imageSaver)
        {
            _repository = repository;
            _imageSaver = imageSaver;
        }

        public ChatLastMessageDto GetLastChatMessageByChatId(int chatId,
            int receiverId)
        {
            var msg = _repository.ChatMessages
                .GetLastMessageByChatIdWithPagination(chatId,
                    ChatLastMessageDto.Selector(receiverId));
            /*var act = _mapper.Map<ChatLastMessageDto>(msg, opt => opt
                .Items["receiverId"] = receiverId);*/
            return msg;
        }

        public IEnumerable<ChatMessageDto>
            GetMessageByReceiverIdWithPagination(int receiverId, int issuerId, int page, int length)
        {
            var chatMessages = _repository.ChatMessages
                .GetMessageByReceiverIdWithPagination(receiverId,
                    issuerId, page, length, ChatMessageDto.Selector);
            //var reversedChatMessages = chatMessages.Reverse();
            return chatMessages;
        }

        public IEnumerable<ChatLastMessageDto>
            GetLastMessageByReceiverIdWithPagination(int receiverId, int page, int length)
        {
            var msgs = _repository.ChatMessages
                .GetLastMessagesByParticipantIdWithPagination(receiverId, page, length,
                    ChatLastMessageDto.Selector(receiverId));
            var dialogs = msgs.GroupBy(x => x.ChatId,
                    (key, g) => g.OrderBy(e => e.ChatId).First());
            return dialogs;
        }

        public async Task<ChatMessage> CreateAndReturnMessageAsync(
            string text, IFormFile image,
            int chatId, int authorId, int receiverId)
        {
            if(text == null && image == null) throw new EmptyMessageException();
            if (text == null && image != null) text = "";
            string imagePath = null;
            if (image != null)
            {
                imagePath = await _imageSaver.SaveAndReturnImagePath(image,
                    "ChatPhoto", chatId);
                var newImage = new Image
                {
                    Path = imagePath,
                    PublishDate = DateTime.Now
                };
                await _repository.Photo.Add(newImage);
            }
            var newMessage = new ChatMessage
            {
                Content = text,
                ImagePath = imagePath,
                UserId = authorId,
                Date = DateTime.Now,
                ReceiverId = receiverId
            };
            _repository.ChatMessages.Add(chatId, newMessage);
            await _repository.SaveAsync();
            return newMessage;
        }

        public async Task<InterlocutorsDto> GetByInterlocutorsIdentifiers(int 
        firstParticipantId, int secondParticipantId)
        {
            var data = await _repository.Chat.GetByInterlocutorsIdentifiers(
                firstParticipantId, secondParticipantId, InterlocutorsDto.Selector());
            return data;
        }
    }
}