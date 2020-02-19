using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.ChatDtos.ClientDtos;
using BE.Dtos.ChatDtos.ServerDtos;
using BE.Features.Chat.Dtos;
using BE.Helpers;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Chat.Services
{
    public interface IChatService
    {
        ChatLastMessageDto GetLastChatMessageByChatId(int chatId, int receiverId);

        IEnumerable<ChatMessageDto> GetMessageByReceiverIdWithPagination(
            int receiverId, int issuerId, int page);

        IEnumerable<ChatLastMessageDto> GetLastMessageByReceiverIdWithPagination(
            int receiverId, int page);

        Task<ChatMessage> CreateAndReturnMessageAsync(NewMessageDto chatMessage,
            int chatId, int authorId, int receiverId);

        Task<InterlocutorsDto> GetByInterlocutorsIdentifiers(int firstParticipantId,
            int secondParticipantId);
    }

    public class ChatService : IChatService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public ChatService(IRepositoryWrapper repository, IMapper mapper,
            IImageSaver imageSaver)
        {
            _repository = repository;
            _mapper = mapper;
            _imageSaver = imageSaver;
        }

        public ChatLastMessageDto GetLastChatMessageByChatId(int chatId,
            int receiverId)
        {
            var msg = _repository.ChatMessages
                .GetLastMessageByChatIdWithPagination(chatId,
                    ChatLastMessageDto.Selector(receiverId));
            var act = _mapper.Map<ChatLastMessageDto>(msg, opt => opt
                .Items["receiverId"] = receiverId);
            return act;
        }

        public IEnumerable<ChatMessageDto>
            GetMessageByReceiverIdWithPagination(int receiverId, int issuerId, int page)
        {
            var chatMessages = _repository.ChatMessages
                .GetMessageByReceiverIdWithPagination(receiverId,
                    issuerId, page, ChatMessageDto.Selector);
            return chatMessages;
        }

        public IEnumerable<ChatLastMessageDto>
            GetLastMessageByReceiverIdWithPagination(int receiverId, int page)
        {
            var msgs = _repository.ChatMessages
                .GetLastMessagesByReceiverIdWithPagination(receiverId, page,
                    ChatLastMessageDto.Selector(receiverId));
            var sortedMessages = msgs.GroupBy(x => x.ChatId,
                (key, g) => g.OrderBy(e => e.ChatId).First());
            return sortedMessages;
        }

        public async Task<ChatMessage> CreateAndReturnMessageAsync(
            NewMessageDto chatMessage,
            int chatId, int authorId, int receiverId)
        {
            string imagePath = null;
            if (chatMessage.File != null)
            {
                imagePath = await _imageSaver.SaveAndReturnImagePath(chatMessage.File,
                    "ChatPhoto", chatId);
                var image = new Image
                {
                    Path = imagePath,
                    PublishDate = DateTime.Now
                };
                await _repository.Photo.Add(image);
            }

            var newMessage = new ChatMessage
            {
                Content = chatMessage.Content,
                ImagePath = imagePath,
                UserId = authorId,
                Date = DateTime.Now,
                ReceiverId = receiverId
            };
            _repository.ChatMessage.Add(newMessage);
            var chatMessages = new ChatMessages
            {
                ChatId = chatId,
                MessageId = newMessage.Id
            };
            _repository.ChatMessages.Add(chatMessages);
            await _repository.SaveAsync();
            return newMessage;
        }

        public async Task<InterlocutorsDto> GetByInterlocutorsIdentifiers(int 
        firstParticipantId,
            int secondParticipantId)
        {
            return await _repository.Chat.GetByInterlocutorsIdentifiers(
                firstParticipantId, secondParticipantId, InterlocutorsDto.Selector());
        }
    }
}