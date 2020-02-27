using System;
using AutoMapper;
using BE.Features.Chat.Dtos;
using BE.Models;

namespace BE.Mapping.Resolvers
{
    public class WrittenByRequestIssuerResolver : IValueResolver<ChatMessages,
        ChatLastMessageDto, bool>
    {
        public bool Resolve(ChatMessages source, ChatLastMessageDto destination, bool
            member, ResolutionContext context)
        {
            var receiverId = Convert.ToInt32(context.Items["receiverId"]);
            return receiverId == source.Message.UserId;
        }
    }
}