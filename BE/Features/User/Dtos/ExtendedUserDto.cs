using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BE.Models;

namespace BE.Features.User.Dtos
{
    public class ExtendedUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public City City { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string ProfileBg { get; set; }
        public string Status { get; set; }
        public int? EducationId { get; set; }
        public Session Session { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? AlcoholAttitudeId { get; set; }
        public int? SmokingAttitudeId { get; set; }
        public IEnumerable<object> UserInterests { get; set; }

        public static Expression<Func<Models.User, ExtendedUserDto>> Selector()
        {
            return e => new ExtendedUserDto
            {
                Id = e.Id,
                City = e.City,
                Name = e.Name,
                Surname = e.Surname,
                Email = e.Email,
                GenderId = e.Gender.Id,
                Birthday = e.Birthday,
                Avatar = e.Avatar,
                ProfileBg = e.ProfileBg,
                Status = e.Status,
                Session = e.Session,
                EducationId = e.EducationId,
                MaritalStatusId = e.AdditionalInfo.MaritalStatus.Id,
                ReligionId = e.AdditionalInfo.Religion.Id,
                AlcoholAttitudeId = e.AdditionalInfo.AlcoholAttitude.Id,
                SmokingAttitudeId = e.AdditionalInfo.SmokingAttitude.Id,
                UserInterests = e.UserInterests.Select(b => new
                {
                    b.Interest.Id,
                    b.Interest.Title
                })
            };
        }
    }
}