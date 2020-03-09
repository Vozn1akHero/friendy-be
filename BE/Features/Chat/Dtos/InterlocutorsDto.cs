using System;
using System.Linq.Expressions;

namespace BE.Features.Chat.Dtos
{
    public class InterlocutorsDto
    {
        public int Id { get; set; }
        public Models.User FirstInterlocutor { get; set; }
        public Models.User SecondInterlocutor { get; set; }

        public static Expression<Func<Models.Chat, InterlocutorsDto>> Selector()
        {
            return e => new InterlocutorsDto
            {
                Id = e.Id,
                FirstInterlocutor = new Models.User
                {
                    Id = e.FirstParticipant.Id,
                    Name = e.FirstParticipant.Name,
                    Avatar = e.FirstParticipant.Avatar,
                    Surname = e.FirstParticipant.Surname,
                    Birthday = e.FirstParticipant.Birthday,
                    City = e.FirstParticipant.City,
                    Session = e.FirstParticipant.Session
                },
                SecondInterlocutor = new Models.User
                {
                    Id = e.SecondParticipant.Id,
                    Name = e.SecondParticipant.Name,
                    Avatar = e.SecondParticipant.Avatar,
                    Surname = e.SecondParticipant.Surname,
                    Birthday = e.SecondParticipant.Birthday,
                    City = e.SecondParticipant.City,
                    Session = e.SecondParticipant.Session
                }
            };
        }
    }
}