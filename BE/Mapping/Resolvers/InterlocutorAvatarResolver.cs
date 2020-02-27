using System;
using AutoMapper;
using BE.Features.Chat.Dtos;
using BE.Models;

namespace BE.Mapping.Resolvers
{
    public class InterlocutorAvatarResolver : IValueResolver<ChatMessages,
        ChatLastMessageDto, string>
    {
        public string Resolve(ChatMessages source, ChatLastMessageDto destination, string
            member, ResolutionContext context)
        {
            var receiverId = Convert.ToInt32(context.Items["receiverId"]);
            return receiverId == source.Message.ReceiverId
                ? source.Message.User.Avatar
                : source.Message.Receiver.Avatar;
        }
    }
}