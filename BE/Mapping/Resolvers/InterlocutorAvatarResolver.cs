using System;
using AutoMapper;
using BE.Dtos.ChatDtos;
using BE.Models;

namespace BE.AutoMapper.Resolvers
{
    public class InterlocutorAvatarResolver : IValueResolver<ChatMessages,
        ChatLastMessageDto, string>
    {
        public string Resolve(ChatMessages source, ChatLastMessageDto destination, string
            member, ResolutionContext context)
        {
            int receiverId = Convert.ToInt32(context.Items["receiverId"]);
            return receiverId == source.Message.ReceiverId
                ? source.Message.User.Avatar
                : source.Message.Receiver.Avatar;
        }
    }
}