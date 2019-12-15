using System;
using System.Collections.Generic;

namespace BE.Dtos
{
    public class UsersLookUpCriteriaDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int EducationId { get; set; }
/*        public string School { get; set; }
        public string University { get; set; }*/
        /*public int AgeMin { get; set; }
        public int AgeMax { get; set; }*/
        public DateTime? BirthdayMin { get; set; }
        public DateTime? BirthdayMax { get; set; }
        public int GenderId { get; set; }
        public int MaritalStatusId { get; set; }
        public int ReligionId { get; set; }
        public int AlcoholOpinionId { get; set; }
        public int SmokingOpinionId { get; set; }
        public IEnumerable<string> Interests { get; set; }
    }
}