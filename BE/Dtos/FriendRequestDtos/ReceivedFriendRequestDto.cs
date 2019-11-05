namespace BE.Dtos.FriendRequestDto
{
    public class ReceivedFriendRequestDto
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RequestId { get; set; }
    }
}