using System;
using System.Linq.Expressions;

namespace BE.Features.Event.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public decimal EntryPrice { get; set; }
        public int CreatorId { get; set; }
        public string Description { get; set; }
        public string AvatarPath { get; set; }
        public string BackgroundPath { get; set; }
        public int ParticipantsAmount { get; set; }
        public int CurrentParticipantsAmount { get; set; }
        public DateTime Date { get; set; }

        public static Expression<Func<Models.Event, EventDto>> Selector
        {
            get
            {
                return e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Street = e.Street,
                    StreetNumber = e.StreetNumber,
                    City = e.City,
                    EntryPrice = e.EntryPrice,
                    CreatorId = e.CreatorId,
                    Description = e.Description,
                    AvatarPath = e.Avatar,
                    BackgroundPath = e.Background,
                    ParticipantsAmount = e.ParticipantsAmount,
                    CurrentParticipantsAmount = e.EventParticipants.Count,
                    Date = e.Date
                };
            }
        }
    }
}