using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Services.Global;

namespace BE.Services.Model
{
    public interface IChatService
    {
        Task<ChatLastMessageDto> GetLastChatMessageByChatId(int chatId);

        Task<IEnumerable<ChatLastMessageDto>>
            GetLastChatMessageRangeByReceiverId
            (int receiverId, int startIndex, int length);
    }

    public class ChatService : IChatService
    {
        private readonly IRawSqlQuery _rawSqlQuery;
        private readonly IRepositoryWrapper _repository;

        public ChatService(IRawSqlQuery rawSqlQuery, IRepositoryWrapper repository)
        {
            _rawSqlQuery = rawSqlQuery;
            _repository = repository;
        }

        public async Task<ChatLastMessageDto> GetLastChatMessageByChatId(int chatId)
        {
            var query =
                "select top 1 c.id as c_chat_id, cm.id, cm.content, cm.image_path, cm.date, cm.user_id as sender, " +
                "u.avatar as senderAvatar, cm.receiver_id as receiver, (select u.avatar from [dbo].[user] u " +
                "where u.id = cm.receiver_id) as receiverAvatar, (select u.id from [dbo].[user] u " +
                "join chat c on u.id = (select case when first_participant_id <> cm.receiver_id then first_participant_id " +
                "when second_participant_id <> cm.receiver_id then second_participant_id end from chat) where u.id <> cm.receiver_id) as interlocutorId, " +
                "(select u.avatar from [dbo].[user] u join chat c on u.id = (select case when first_participant_id <> cm.receiver_id " +
                "then first_participant_id when second_participant_id <> cm.receiver_id then second_participant_id end from chat) " +
                "where u.id <> cm.receiver_id) as interlocutorAvatar from chat c join chat_messages cms on c.id = cms.chat_id " +
                "join chat_message cm on cms.message_id = cm.id join [dbo].[user] u on cm.user_id = u.id where c.id = 1 order by date desc";
            var results = _rawSqlQuery.Execute(query, e => new ChatLastMessageDto
            {
                ChatId = (int) e[0],
                Id = (int) e[1],
                Content = e[2] == DBNull.Value ? null : (string) e[2],
                ImageUrl = e[3] == DBNull.Value ? null : (string) e[3],
                Date = (DateTime) e[4],
                SenderId = (int) e[5],
                SenderAvatarPath = (string) e[6],
                ReceiverId = (int) e[7],
                ReceiverAvatarPath = (string) e[8],
                InterlocutorId = (int) e[9],
                InterlocutorAvatarPath = (string) e[10],
                WrittenByRequestIssuer = true
            });
            return results[0];
        }

        public async Task<IEnumerable<ChatLastMessageDto>>
            GetLastChatMessageRangeByReceiverId
            (int receiverId, int startIndex, int length)
        {
            var query =
                "with every_chat_last_messages as (select c.id as c_chat_id, cm.id, cm.content, cm.image_path, cm.date, " +
                "cm.user_id as sender, u.avatar as senderAvatar, cm.receiver_id as receiver, " +
                "row_number() over (partition by c.id order by date desc) as record_number " +
                "from chat c join chat_messages cms on c.id = cms.chat_id join chat_message cm on cms.message_id = cm.id " +
                $"join [dbo].[user] u on cm.user_id = u.id where c.first_participant_id = {receiverId} or c.second_participant_id = {receiverId} and c.id >= {startIndex}) " +
                $"select top {length} c_chat_id, id, content, image_path, date, sender, senderAvatar, receiver, (select u.avatar from [dbo].[user] u where u.id = receiver) as receiverAvatar, " +
                $"(select u.id from [dbo].[user] u join chat c on u.id = (select case when first_participant_id <> {receiverId} then first_participant_id when second_participant_id <> {receiverId} then second_participant_id end from chat) where u.id <> {receiverId}) as interlocutor, " +
                $"(select u.avatar from [dbo].[user] u join chat c on u.id = (select case when first_participant_id <> {receiverId} then first_participant_id when second_participant_id <> {receiverId} then second_participant_id end from chat) where u.id <> {receiverId}) as interlocutorAvatar " +
                "from every_chat_last_messages where record_number = 1 order by id desc";
            var messages = _rawSqlQuery.Execute(query, e => new ChatLastMessageDto
            {
                ChatId = (int) e[0],
                Id = (int) e[1],
                Content = e[2] == DBNull.Value ? null : (string) e[2],
                ImageUrl = e[3] == DBNull.Value ? null : (string) e[3],
                Date = (DateTime) e[4],
                SenderId = (int) e[5],
                SenderAvatarPath = (string) e[6],
                ReceiverId = (int) e[7],
                ReceiverAvatarPath = (string) e[8],
                InterlocutorId = (int) e[9],
                InterlocutorAvatarPath = (string) e[10],
                WrittenByRequestIssuer = (int) e[5] == receiverId
            });
            return messages;
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessageRangeByReceiverId(
            int receiverId, int issuerId, int startIndex, int length)
        {
            var chatMessages = await _repository.ChatMessages
                .GetMessageRangeByReceiverId(receiverId, issuerId, startIndex, length);
            var res = chatMessages.Select(e => new ChatMessageDto
            {
                Content = e.Message.Content,
                ImagePath = e.Message.ImagePath,
                UserId = e.Message.UserId,
                Date = e.Message.Date
            }).Reverse();
            return res;
        }
    }
}