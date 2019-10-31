using System;

namespace BE.Dtos
{
    public class UsersLookUpCriteriaDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Education { get; set; }
        public string School { get; set; }
        public string University { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public int Gender { get; set; }
        public int FamilyStatus { get; set; }
        public int Religion { get; set; }
        public int AlcoholOpinion { get; set; }
        public int SmokingOpinion { get; set; }
        public int DrugsOpinion { get; set; }
        public string[] Interests { get; set; }
    }
}