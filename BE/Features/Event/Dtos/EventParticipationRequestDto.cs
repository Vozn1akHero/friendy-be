using System;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.Event.Dtos
{
    public class EventParticipationRequestDto
    {
        public int Id { get; set; }
        public int IssuerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }

        public static Expression<Func<EventParticipationRequest, 
            EventParticipationRequestDto>> Selector()
        {
            return e => new EventParticipationRequestDto
            {
                Id = e.Id, 
                IssuerId = e.IssuerId,
                Name = e.Issuer.Name,
                Surname = e.Issuer.Surname,
                Avatar = e.Issuer.Avatar
            };
        }
    }
}