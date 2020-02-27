using System;
using AutoMapper;
using BE.Features.Chat.Dtos;
using BE.Models;

namespace BE.Mapping.Resolvers
{
    public class InterlocutorIdResolver : IValueResolver<ChatMessages,
        ChatLastMessageDto, int>
    {
        public int Resolve(ChatMessages source, ChatLastMessageDto destination, int
            member, ResolutionContext context)
        {
            var receiverId = Convert.ToInt32(context.Items["receiverId"]);
            return receiverId == source.Message.ReceiverId
                ? source.Message.UserId
                : source.Message.ReceiverId;
        }
    }
}