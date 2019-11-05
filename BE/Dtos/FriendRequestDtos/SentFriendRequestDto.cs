namespace BE.Dtos.FriendRequestDto
{
    public class SentFriendRequestDto
    {
        public int ReceiverId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RequestId { get; set; }
    }
}