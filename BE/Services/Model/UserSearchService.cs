using System;
using System.Collections.Generic;
using BE.Dtos;
using BE.ElasticSearch;
using Nest;

namespace BE.Services.Model
{
    public interface IUserSearchService
    {
        IEnumerable<ExtendedUserDto> SearchByCriteria(UsersLookUpCriteriaDto usersLookUpCriteriaDto);
    }

    public class UserSearchService : IUserSearchService
    {
        private readonly ElasticClient _client;

        public UserSearchService(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }

        public IEnumerable<ExtendedUserDto> SearchByCriteria(UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var searchResponse = _client.Search<ExtendedUserDto>(s => s
                    .Query(q => q.Wildcard(m => m
                        .Field(f => f.Name)
                        .Value(usersLookUpCriteriaDto.Name + "*")))
                    .Query(q => q.Wildcard(m => m.Field(f => f.Surname)
                        .Value(usersLookUpCriteriaDto.Surname + "*")))
                    .Query(q => q.Wildcard(m => m.Field(f => f.City)
                        .Value(usersLookUpCriteriaDto.City + "*")))
                    /*.Query(q => q.Match(m => m
                        .Field(f => f.ReligionId)
                        .Query(Convert.ToString(usersLookUpCriteriaDto.ReligionId))))
                    .Query(q => q.Match(m => m
                        .Field(f => f.EducationId)
                        .Query(Convert.ToString(usersLookUpCriteriaDto.EducationId))))
                    .Query(q => q.Match(m => m
                                .Field(f => f.GenderId)
                                .Query(Convert.ToString(usersLookUpCriteriaDto.GenderId))))
                    .Query(q => q.Match(m => m
                        .Field(f => f.MaritalStatusId)
                        .Query(Convert.ToString(usersLookUpCriteriaDto.MaritalStatusId))))
                    .Query(q => q.Match(m => m
                        .Field(f => f.AlcoholAttitudeId)
                        .Query(Convert.ToString(usersLookUpCriteriaDto.AlcoholOpinionId))))
                    .Query(q => q.Match(m => m  
                        .Field(f => f.SmokingAttitudeId)
                        .Query(Convert.ToString(usersLookUpCriteriaDto.SmokingOpinionId))
                    ))*/
                    .Query(q => q.Match(m => m
                            .Field(f => f.EducationId)
                            .Query(Convert.ToString(usersLookUpCriteriaDto.EducationId))
                            .Field(f => f.GenderId)
                            .Query(Convert.ToString(usersLookUpCriteriaDto.GenderId))
                            .Field(f => f.MaritalStatusId)
                            .Query(Convert.ToString(usersLookUpCriteriaDto.MaritalStatusId))
                            .Field(f => f.ReligionId)
                            .Query(Convert.ToString(usersLookUpCriteriaDto.ReligionId))
                            .Field(f => f.AlcoholAttitudeId)
                            .Query(Convert.ToString(usersLookUpCriteriaDto.AlcoholOpinionId))
                            .Field(f => f.SmokingAttitudeId)
                            .Query(Convert.ToString(usersLookUpCriteriaDto.SmokingOpinionId))
                    ))
                    .Query(q => q.DateRange(m => m
                        .Field(f => f.Birthday)
                        .GreaterThanOrEquals(usersLookUpCriteriaDto.BirthdayMin)
                        .LessThanOrEquals(usersLookUpCriteriaDto.BirthdayMax)
                    ))
            );
            var users = searchResponse.Documents;
            return users;
        }

        private int CalculateAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;
            if (birthday.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}