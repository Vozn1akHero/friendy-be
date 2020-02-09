using System;
using AutoMapper;
using BE.Dtos.ChatDtos;
using BE.Models;

namespace BE.AutoMapper.Resolvers
{
    public class InterlocutorIdResolver: IValueResolver<ChatMessages, 
    ChatLastMessageDto, int>
    {
        public int Resolve(ChatMessages source, ChatLastMessageDto destination, int 
        member, ResolutionContext context)
        {
            int receiverId = Convert.ToInt32(context.Items["receiverId"]);
            return receiverId == source.Message.ReceiverId
                ? source.Message.UserId
                : source.Message.ReceiverId;
        }
    }
}