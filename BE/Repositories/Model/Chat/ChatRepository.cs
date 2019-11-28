using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using BE.Services.Global.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Chat
{
    public class ChatRepository: RepositoryBase<Models.Chat>, IChatRepository
    {
        private IRowSqlQueryService _rowSqlQueryService;
        
        public ChatRepository(FriendyContext friendyContext, 
            IRowSqlQueryService rowSqlQueryService) : base(friendyContext)
        {
            _rowSqlQueryService = rowSqlQueryService;
        }

        public async Task Add(int firstParticipantId, int secondParticipantId)
        {
            var newChat = new Models.Chat()
            {
                FirstParticipantId = firstParticipantId,
                SecondParticipantId = secondParticipantId
            };
            Create(newChat);
            await SaveAsync();
        }

        public async Task<Models.Chat> GetByInterlocutorsIdentifiers(int firstParticipantId, int secondParticipantId)
        {
            return await FindByCondition(e =>
                    e.FirstParticipantId == firstParticipantId && e.SecondParticipantId == secondParticipantId
                    || e.FirstParticipantId == secondParticipantId && e.SecondParticipantId == firstParticipantId)
                .SingleOrDefaultAsync();
        }

        public async Task<ChatLastMessageDto> GetLastChatMessageByChatId(int chatId)
        {
            string query = $"select top 1 c.id as c_chat_id, cm.id, cm.content, cm.image_path, cm.date, cm.user_id as sender, " +
                           $"u.avatar as senderAvatar, cm.receiver_id as receiver, (select u.avatar from [dbo].[user] u " +
                           $"where u.id = cm.receiver_id) as receiverAvatar, (select u.id from [dbo].[user] u " +
                           $"join chat c on u.id = (select case when first_participant_id <> cm.receiver_id then first_participant_id " +
                           $"when second_participant_id <> cm.receiver_id then second_participant_id end from chat) where u.id <> cm.receiver_id) as interlocutorId, " +
                           $"(select u.avatar from [dbo].[user] u join chat c on u.id = (select case when first_participant_id <> cm.receiver_id " +
                           $"then first_participant_id when second_participant_id <> cm.receiver_id then second_participant_id end from chat) " +
                           $"where u.id <> cm.receiver_id) as interlocutorAvatar from chat c join chat_messages cms on c.id = cms.chat_id " +
                           $"join chat_message cm on cms.message_id = cm.id join [dbo].[user] u on cm.user_id = u.id where c.id = 1 order by date desc";
            var results = _rowSqlQueryService.Execute(query, e => new ChatLastMessageDto
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
                WrittenByRequestIssuer = true
            });
            return results[0];
        }
    }
}