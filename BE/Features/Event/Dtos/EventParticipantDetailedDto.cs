using System;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Event.Dtos
{
    public class EventParticipantDetailedDto
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Session Session { get; set; }
        public bool IsAdmin { get; set; }

        public static Expression<Func<EventParticipants, EventParticipantDetailedDto>> Selector
        {
           get{
               return e => new EventParticipantDetailedDto
               {
                   Id = e.ParticipantId,
                   AvatarPath = e.Participant.Avatar,
                   Name = e.Participant.Name,
                   Surname = e.Participant.Surname,
                   Session = e.Participant.Session,
                   IsAdmin = e.Event.EventAdmins.Any(d => d.UserId == e.ParticipantId)
               };
           }
        }
    }
}