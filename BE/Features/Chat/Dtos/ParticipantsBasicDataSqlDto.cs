namespace BE.Features.Chat.Dtos
{
    public class ParticipantsBasicDataSqlDto
    {
        public int FriendId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }
    }
}