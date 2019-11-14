namespace BE.Dtos.FriendDtos
{
    public class FriendDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool OnlineStatus { get; set; }
        public string DialogLink { get; set; }
        public string AvatarPath { get; set; }
    }
}