using System;

namespace BE.Dtos
{
    public class UserBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public int BirthYear { get; set; }
        public int Birthday { get; set; }
        public int BirthMonth { get; set; }
        public byte[] Avatar { get; set; }
        public string Status { get; set; }
    }
}