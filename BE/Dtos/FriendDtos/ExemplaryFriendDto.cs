using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace BE.Dtos.FriendDtos
{
    public class ExemplaryFriendDto
    {
        public int Id { get; set; }
        public byte[] Avatar { get; set; }
    }
}