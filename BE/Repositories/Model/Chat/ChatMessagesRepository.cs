using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using BE.Services.Global.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BE.Repositories.Chat
{
    public class ChatMessagesRepository : RepositoryBase<ChatMessages>, IChatMessagesRepository
    {
        private IRowSqlQueryService _rowSqlQueryService;
        
        public ChatMessagesRepository(FriendyContext friendyContext,
            IRowSqlQueryService rowSqlQueryService) : base(friendyContext)
        {
            _rowSqlQueryService = rowSqlQueryService;
        }

        public async Task Add(ChatMessages chatMessages)
        {
            Create(chatMessages);
            await SaveAsync();
        }

        public async Task<List<ChatLastMessageDto>> GetLastChatMessageRangeByReceiverId(int receiverId, int startIndex, int length)
        {
            string query = $"with every_chat_last_messages as (select c.id as c_chat_id, cm.id, cm.content, cm.image_path, cm.date, " +
                           $"cm.user_id as sender, u.avatar as senderAvatar, cm.receiver_id as receiver, " +
                           $"row_number() over (partition by c.id order by date desc) as record_number " +
                           $"from chat c join chat_messages cms on c.id = cms.chat_id join chat_message cm on cms.message_id = cm.id " +
                           $"join [dbo].[user] u on cm.user_id = u.id where c.first_participant_id = {receiverId} or c.second_participant_id = {receiverId} and c.id >= {startIndex}) " +
                           $"select top {length} c_chat_id, id, content, image_path, date, sender, senderAvatar, receiver, (select u.avatar from [dbo].[user] u where u.id = receiver) as receiverAvatar, " +
                           $"(select u.id from [dbo].[user] u join chat c on u.id = (select case when first_participant_id <> {receiverId} then first_participant_id when second_participant_id <> {receiverId} then second_participant_id end from chat) where u.id <> {receiverId}) as interlocutor, " +
                           $"(select u.avatar from [dbo].[user] u join chat c on u.id = (select case when first_participant_id <> {receiverId} then first_participant_id when second_participant_id <> {receiverId} then second_participant_id end from chat) where u.id <> {receiverId}) as interlocutorAvatar " +
                           $"FROM every_chat_last_messages WHERE record_number = 1";
            var messages = _rowSqlQueryService.Execute(query, e => new ChatLastMessageDto
            {
                ChatId = (int)e[0],
                Id = (int)e[1],
                Content = e[2] == DBNull.Value ? null : (string)e[2],
                ImageUrl = e[3] == DBNull.Value ? null : (string)e[3],
                Date = (DateTime)e[4],
                SenderId = (int)e[5],
                SenderAvatarPath = (string)e[6],
                ReceiverId = (int)e[7],
                ReceiverAvatarPath = (string)e[8],
                InterlocutorId = (int)e[9],
                InterlocutorAvatarPath = (string)e[10],
                WrittenByRequestIssuer = (int)e[5] == receiverId
            });
            return messages;
        }

        public async Task<List<ChatMessageDto>> GetMessageRangeByReceiverId(int receiverId, int issuerId, int startIndex, int length)
        {
            var chatMessages = await FindByCondition(e => (e.Chat.FirstParticipantId == receiverId 
                                                           && e.Chat.SecondParticipantId == issuerId) 
                                                          || (e.Chat.FirstParticipantId == issuerId 
                                                              && e.Chat.SecondParticipantId == receiverId) 
                                                          && e.MessageId >= startIndex).Take(length)
                .Select(e => new ChatMessageDto
                {
                    Content = e.Message.Content,
                    ImagePath = e.Message.ImagePath,
                    UserId = e.Message.UserId,
                    Date = e.Message.Date
                }).ToListAsync();
            
            return chatMessages;
        }
    }
}