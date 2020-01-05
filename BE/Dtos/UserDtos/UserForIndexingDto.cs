using System;
using System.Collections.Generic;

namespace BE.Dtos
{
    public class UserForIndexingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public int? EducationId { get; set; }
        public int GenderId { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? AlcoholAttitudeId { get; set; }
        public int? SmokingAttitudeId { get; set; }
        public IEnumerable<UserInterestForElasticsearchDto> UserInterests { get; set; }
    }
}