using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.ElasticSearch;
using BE.Features.User.Dtos;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace BE.Features.Search.Services
{
    public interface IUserSearchService
    {
        IEnumerable<ExtendedUserDto> SearchByCriteria(
            UsersLookUpCriteriaDto usersLookUpCriteriaDto);

        Task<IEnumerable<Interest>> FindInterestsByKeyword(string keyword);
    }

    public class UserSearchService : IUserSearchService
    {
        private readonly ElasticClient _client;
        private readonly FriendyContext _friendyContext;

        public UserSearchService(ElasticClientProvider provider,
            FriendyContext friendyContext)
        {
            _friendyContext = friendyContext;
            _client = provider.Client;
        }

        public IEnumerable<ExtendedUserDto> SearchByCriteria(
            UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var searchResponse = _client.Search<ExtendedUserDto>(s => s
                .Query(q => q.Bool(m => m.Must(bs =>
                    bs.MatchPhrase(f => f.Field(d => d.Name).Query(usersLookUpCriteriaDto
                        .Name))
                    && bs.MatchPhrase(f => f.Field(d => d.Surname).Query
                    (usersLookUpCriteriaDto
                        .Surname))
                    && bs.MatchPhrase(f => f.Field(d => d.City).Query
                    (usersLookUpCriteriaDto
                        .City))
                    && bs.Match(f => f.Field(d => d.EducationId).Query
                    (usersLookUpCriteriaDto
                        .EducationId.ToString()))
                    && bs.DateRange(f => f.LessThan(usersLookUpCriteriaDto.BirthdayMax)
                        .GreaterThan(usersLookUpCriteriaDto.BirthdayMin))
                    && bs.Match(f => f.Field(d => d.MaritalStatusId).Query
                    (usersLookUpCriteriaDto
                        .MaritalStatusId.ToString()))
                    && bs.Match(f => f.Field(d => d.AlcoholAttitudeId).Query
                    (usersLookUpCriteriaDto
                        .AlcoholOpinionId.ToString()))
                    && bs.Match(f => f.Field(d => d.ReligionId).Query
                    (usersLookUpCriteriaDto
                        .ReligionId.ToString()))
                    && bs.Match(f => f.Field(d => d.SmokingAttitudeId).Query
                    (usersLookUpCriteriaDto
                        .SmokingOpinionId.ToString()))
                    && bs.Terms(f=>f.Field(d=>d.UserInterests).Field("id").Terms
                    (usersLookUpCriteriaDto.Interests)))
                )));
            var users = searchResponse.Documents;
            return users;
        }

        public async Task<IEnumerable<Interest>> FindInterestsByKeyword(string keyword)
        {
            var i = await _friendyContext.Interest.Where(e => e.Title.StartsWith(keyword))
                .ToListAsync();
            return i;
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