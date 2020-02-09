using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatMessagesRepository : IRepositoryBase<ChatMessages>
    {
        Task Add(ChatMessages chatMessages);
        Task<IEnumerable<TType>> GetMessageByReceiverIdWithPaginationAsync<TType>(
            int receiverId, int issuerId, int page, Expression<Func<ChatMessages, TType>> selector);
        Task<IEnumerable<TType>> GetLastMessagesByReceiverIdWithPaginationAsync<TType>(
            int receiverId, int page, Expression<Func<ChatMessages, TType>> selector);
        Task<TType>
            GetLastMessageByChatIdWithPaginationAsync<TType>(int chatId, Expression<Func<ChatMessages, TType>> selector);
    }
}