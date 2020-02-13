using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.ChatDtos;
using BE.Interfaces;

namespace BE.Services.Model
{
    public interface IChatService
    {
        Task<ChatLastMessageDto> GetLastChatMessageByChatId(int chatId, int receiverId);
        Task<IEnumerable<ChatMessageDto>> GetMessageByReceiverIdWithPagination(
            int receiverId, int issuerId, int page);
        Task<IEnumerable<ChatLastMessageDto>> GetLastMessageByReceiverIdWithPagination(
            int receiverId, int page);
    }

    public class ChatService : IChatService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ChatService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<ChatLastMessageDto> GetLastChatMessageByChatId(int chatId,
            int receiverId)
        {
            var msg = await _repository.ChatMessages
                .GetLastMessageByChatIdWithPaginationAsync(chatId, ChatLastMessageDto.Selector(receiverId));
            var act = _mapper.Map<ChatLastMessageDto>(msg, opt => opt
                .Items["receiverId"] = receiverId);
            return act;
        }

        public async Task<IEnumerable<ChatMessageDto>>
            GetMessageByReceiverIdWithPagination(int receiverId, int issuerId, int page)
        {
            var chatMessages = await _repository.ChatMessages
                .GetMessageByReceiverIdWithPaginationAsync(receiverId,
                    issuerId, page, ChatMessageDto.Selector);
            return chatMessages;
        }

        public async Task<IEnumerable<ChatLastMessageDto>>
            GetLastMessageByReceiverIdWithPagination(int receiverId, int page)
        {
            var msgs = await _repository.ChatMessages
                .GetLastMessagesByReceiverIdWithPaginationAsync(receiverId, page,
                    ChatLastMessageDto.Selector(receiverId));
            var sortedMessages = msgs.GroupBy(x => x.ChatId,
                (key, g) => g.OrderBy(e => e.ChatId).First());
            return sortedMessages;
        }
    }
}