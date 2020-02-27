using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Chat.Repositories
{
    public interface IChatMessagesRepository : IRepositoryBase<ChatMessages>
    {
        void Add(ChatMessages chatMessages);

        IEnumerable<TType> GetMessageByReceiverIdWithPagination<TType>(
            int receiverId, int issuerId, int page,
            Expression<Func<ChatMessages, TType>> selector);

        IEnumerable<TType> GetLastMessagesByReceiverIdWithPagination<TType>(
            int receiverId, int page, Expression<Func<ChatMessages, TType>> selector);

        TType
            GetLastMessageByChatIdWithPagination<TType>(int chatId,
                Expression<Func<ChatMessages, TType>> selector);
    }
}