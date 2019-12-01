using System;
using System.Collections;
using System.Collections.Generic;
using BE.Models;

namespace BE.Dtos
{
    public class ExtendedUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public int EducationId { get; set; }
        public int MaritalStatusId { get; set; }
        public int ReligionId { get; set; }
        public int AlcoholAttitudeId { get; set; }
        public int SmokingAttitudeId { get; set; }
        public IEnumerable<object> UserInterests { get; set; }
    }
}