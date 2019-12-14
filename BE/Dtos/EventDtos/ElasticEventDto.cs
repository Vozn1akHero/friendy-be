using System;

namespace BE.Dtos.EventDtos
{
    public class ElasticEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public float EntryPrice { get; set; }
        //public string AvatarPath { get; set; }
        //public string BackgroundPath { get; set; }
        public int ParticipantsAmount { get; set; }
        public int CreatorId { get; set; }
        //public int CurrentParticipantsAmount { get; set; }
        public DateTime Date { get; set; }
    }
}