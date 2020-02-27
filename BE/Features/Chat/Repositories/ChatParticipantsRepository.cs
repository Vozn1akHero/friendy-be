namespace BE.Features.Chat.Repositories
{
    /*public class ChatParticipantsRepository : RepositoryBase<ChatParticipants>, IChatParticipantsRepository
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
    }*/
}