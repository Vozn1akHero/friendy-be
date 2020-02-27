using System;

namespace BE.Features.User.Dtos
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