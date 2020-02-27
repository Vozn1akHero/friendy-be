namespace BE.Features.Friendship.Dtos
{
    public class SentFriendRequestDto
    {
        public int ReceiverId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AvatarPath { get; set; }
        public int RequestId { get; set; }
    }
}