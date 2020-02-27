namespace BE.Features.Chat.Dtos
{
    public class FriendBasicDataInDialogDto
    {
        public int FriendId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] Avatar { get; set; }
    }
}