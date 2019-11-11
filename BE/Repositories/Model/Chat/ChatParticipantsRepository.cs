using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;
using BE.Interfaces.Repositories.Chat;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BE.Repositories.Chat
{
    public class ChatParticipantsRepository : RepositoryBase<ChatParticipants>, IChatParticipantsRepository
    {
        private readonly IAvatarConverterService _userAvatarConverterService;
        private readonly ICustomSqlQueryService _customSqlQueryService;

        public ChatParticipantsRepository(FriendyContext friendyContext, 
            IAvatarConverterService userAvatarConverterService,
            ICustomSqlQueryService customSqlQueryService) : base(friendyContext)
        {
            _userAvatarConverterService = userAvatarConverterService;
            _customSqlQueryService = customSqlQueryService;
        }

        public async Task<List<ChatParticipants>> GetUserChatList(int userId)
        {
            var chatList = await FindByCondition(e => e.UserId == userId).ToListAsync();
            return chatList;
        }
        
        public async Task AddNewAfterFriendAdding(int chatId, int[] participants)
        {
            int firstParticipant = participants[0];
            int secondParticipant = participants[1];
            Create(new ChatParticipants
            {
                ChatId = chatId,
                UserId = firstParticipant
            });
            Create(new ChatParticipants
            {
                ChatId = chatId,
                UserId = secondParticipant
            });
            await SaveAsync();
        }

        public async Task<FriendBasicDataInDialogDto> GetFriendBasicDataInDialogByChatId(int chatId, int userId)
        {
            /*string sqlQuery = "select tab.id as FriendId, tab.name as Name, tab.surname as Surname, tab.avatar as Avatar from (select u.id, u.name, u.surname, u.avatar from chat_participants cp join [dbo].[user] u on cp.user_id = u.id where cp.chat_id = @chatId) tab where tab.id <> @userId";

            var friendBasicDataInDialogList = _customSqlQueryService.ExecuteQuery(sqlQuery, new List<object>
            {
                new SqlParameter("chatId", chatId),
                new SqlParameter("userId", userId)
            });
            
            //var chatParticipantCast = friendBasicDataInDialogList.ElementAt(0);

            var friendBasicDataInDialogList = ExecuteSqlQuery(sqlQuery, new List<object>
            {
                new SqlParameter("@chatId", chatId),
                new SqlParameter("@userId", userId)
            });
            
            //byte[] friendAvatar = _userAvatarConverterService.ConvertToByte(chatParticipantCast.GetString(3));
            var friendBasicDataInDialog = new FriendBasicDataInDialogDto
            {
                FriendId = 7
                Name = chatParticipantCast.GetString(1),
                Surname = chatParticipantCast.GetString(2),
                Avatar = friendAvatar
            };
            
            return friendBasicDataInDialog;*/
            
            var chatParticipant = await FindByCondition(e => e.ChatId == chatId)
                .Include(e => e.User)
                .Where(e => e.UserId != userId)
                .Select(e => new FriendBasicDataInDialogDto
                {
                    FriendId = e.User.Id,
                    Name = e.User.Name,
                    Surname = e.User.Surname,
                    Avatar = _userAvatarConverterService.ConvertToByte(e.User.Avatar)
                })
                .SingleOrDefaultAsync();

            return chatParticipant;
        }

        public async Task<List<ParticipantsBasicDataDto>> GetParticipantsBasicDataByChatId(int chatId)
        {
            var participantsBasicData = new List<ParticipantsBasicDataDto>();
            var users = await FindByCondition(e => e.ChatId == chatId)
                .Include(e => e.User)
                .Select(e => new { e.User.Avatar, e.User.Id })
                .ToListAsync();
            foreach (var user in users)
            {
                byte[] friendAvatar = _userAvatarConverterService.ConvertToByte(user.Avatar);
                participantsBasicData.Add(new ParticipantsBasicDataDto
                {
                    UserId = user.Id,
                    Avatar = friendAvatar
                });
            }
            return participantsBasicData;
        }
    }
}