using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessages>,
        IChatMessagesRepository
    {
        public ChatMessagesRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public async Task Add(ChatMessages chatMessages)
        {
            Create(chatMessages);
            await SaveAsync();
        }
        

        public async Task<IEnumerable<TType>>
            GetMessageByReceiverIdWithPaginationAsync<TType>(
                int receiverId, int issuerId, int page,
                Expression<Func<ChatMessages, TType>> selector)
        {
            var length = 20;
            var chatMessages = await FindByCondition(e =>
                    e.Chat.FirstParticipantId == receiverId
                    && e.Chat.SecondParticipantId == issuerId
                    || e.Chat.FirstParticipantId == issuerId
                    && e.Chat.SecondParticipantId == receiverId)
                .Include(e => e.Message)
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * length)
                .Take(length)
                .Select(selector)
                .ToListAsync();
            return chatMessages;
        }

        public async Task<IEnumerable<TType>>
            GetLastMessagesByReceiverIdWithPaginationAsync<TType>(int receiverId,
                int page, Expression<Func<ChatMessages, TType>> selector)
        {
            var length = 20;
            var chatMessages = await FindByCondition(e =>
                    e.Chat.FirstParticipantId == receiverId
                    || e.Chat.SecondParticipantId == receiverId)
                .Include(e => e.Message)
                .Include(e => e.Message.User)
                .Include(e => e.Message.Receiver)
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * length)
                .Take(length)
                .Select(selector)
                .ToListAsync();
            return chatMessages;
        }

        public async Task<TType>
            GetLastMessageByChatIdWithPaginationAsync<TType>(int chatId,
                Expression<Func<ChatMessages, TType>> selector)
        {
            var chatMessage = await FindByCondition(e =>
                    e.ChatId == chatId)
                .OrderByDescending(e => e.MessageId)
                .Select(selector)
                .FirstAsync();
            return chatMessage;
        }
    }
}