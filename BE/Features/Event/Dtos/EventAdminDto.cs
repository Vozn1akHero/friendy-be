using System;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Event.Dtos
{
    public class EventAdminDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Session Session { get; set; }
        public bool IsCreator { get; set; }

        public static Expression<Func<EventAdmins, EventAdminDto>> Selector
        {
            get {
                return e => new EventAdminDto
                {
                    Id = e.UserId,
                    Avatar = e.User.Avatar,
                    Name = e.User.Name,
                    Surname = e.User.Surname,
                    Session = e.User.Session,
                    IsCreator = e.Event.CreatorId == e.UserId
                };
            }
        }
    }
}