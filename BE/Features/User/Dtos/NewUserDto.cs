using System;

namespace BE.Features.User.Dtos
{
    public class NewUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public DateTime Birthday { get; set; }
        public int GenderId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}