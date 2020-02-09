using System;
using AutoMapper;
using BE.Dtos.ChatDtos;
using BE.Models;

namespace BE.AutoMapper.Resolvers
{
    public class WrittenByRequestIssuerResolver : IValueResolver<ChatMessages,
        ChatLastMessageDto, bool>
    {
        public bool Resolve(ChatMessages source, ChatLastMessageDto destination, bool
            member, ResolutionContext context)
        {
            int receiverId = Convert.ToInt32(context.Items["receiverId"]);
            return receiverId == source.Message.UserId;
        }
    }
}