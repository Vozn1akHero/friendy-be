using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Chat.Repositories
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessages>,
        IChatMessagesRepository
    {
        public ChatMessagesRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public void Add(ChatMessages chatMessages)
        {
            Create(chatMessages);
        }


        public IEnumerable<TType>
            GetMessageByReceiverIdWithPagination<TType>(
                int receiverId, int issuerId, int page,
                Expression<Func<ChatMessages, TType>> selector)
        {
            var length = 20;
            var chatMessages = FindByCondition(e =>
                    e.Chat.FirstParticipantId == receiverId
                    && e.Chat.SecondParticipantId == issuerId
                    || e.Chat.FirstParticipantId == issuerId
                    && e.Chat.SecondParticipantId == receiverId)
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * length)
                .Take(length)
                .Select(selector)
                .ToList();
            return chatMessages;
        }

        public IEnumerable<TType>
            GetLastMessagesByReceiverIdWithPagination<TType>(int receiverId,
                int page, Expression<Func<ChatMessages, TType>> selector)
        {
            var length = 20;
            var chatMessages = FindByCondition(e =>
                    e.Chat.FirstParticipantId == receiverId
                    || e.Chat.SecondParticipantId == receiverId)
                .Include(e => e.Message)
                .Include(e => e.Message.User)
                .Include(e => e.Message.Receiver)
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * length)
                .Take(length)
                .Select(selector)
                .ToList();
            return chatMessages;
        }

        public TType GetLastMessageByChatIdWithPagination<TType>(int chatId,
            Expression<Func<ChatMessages, TType>> selector)
        {
            var chatMessage = FindByCondition(e =>
                    e.ChatId == chatId)
                .OrderByDescending(e => e.MessageId)
                .Select(selector)
                .First();
            return chatMessage;
        }

        public void Add(int chatId, ChatMessage chatMessage)
        {
            Create(new ChatMessages
            {
                ChatId = chatId,
                Message = chatMessage
            });
        }
    }
}