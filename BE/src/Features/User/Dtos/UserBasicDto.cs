using System;

namespace BE.Dtos.UserDtos
{
    public class UserBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
    }
}