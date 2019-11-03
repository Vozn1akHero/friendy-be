using System.Collections.Generic;

namespace BE.Dtos
{
    public class UserLookUpModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string School { get; set; }
        public string University { get; set; }
        public int Age { get; set; }
        public int? EducationId { get; set; }
        public int? GenderId { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? AlcoholAttitudeId { get; set; }
        public int? SmokingAttitudeId { get; set; }
        public IEnumerable<string> UserInterests { get; set; }
    }
}