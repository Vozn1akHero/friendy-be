namespace BE.Dtos.ChatDtos
{
    public class DialogBasicDataDto
    {
        public bool FriendHasImage { get; set; }
        public byte[] UserAvatar { get; set; }
        public bool UserHasImage { get; set; }
        public byte[] FriendAvatar { get; set; }
    }
}