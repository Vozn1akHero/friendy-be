namespace BE.Features.Friendship.Dtos
{
    public class FriendDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool OnlineStatus { get; set; }
        public string AvatarPath { get; set; }
    }
}