using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.User.Dtos;
using BE.Models;
using BE.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Search.Services
{
    public interface ISQLServer_UserSearchService
    {
        Task<IEnumerable<UserDto>> SearchByCriteriaAsync(
            UsersLookUpCriteriaDto criteria, int issuerId, int page, int length);
        IEnumerable<UserDto> TrendyUsers(int issuerId, int page, int length);
        Task<IEnumerable<UserDto>> FindByKeywordAsync(string keyword, int issuerId, int page, int length);
    }

    public class SQLServer_UserSearchService : ISQLServer_UserSearchService
    {
        private readonly FriendyContext _friendyContext;
        private IMapper _mapper;

        public SQLServer_UserSearchService(FriendyContext friendyContext,
            IMapper mapper)
        {
            _friendyContext = friendyContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> SearchByCriteriaAsync
            (UsersLookUpCriteriaDto criteria, int issuerId, int page, int length)
        {
            var foundUsers =
                _friendyContext.Set<Models.User>().Where(e => (criteria.Name == null 
                                                               || e.Name.ToUpper() == criteria.Name.ToUpper())
                                                              && (criteria.Surname == null 
                                                                  || e.Surname.ToUpper() == criteria.Surname.ToUpper())
                                                              && (criteria.EducationId == null ||
                                                                  e.EducationId == criteria.EducationId)
                                                              && (criteria.BirthdayMin == null ||
                                                                  e.Birthday > criteria.BirthdayMin)
                                                              && (criteria.BirthdayMax == null ||
                                                                  e.Birthday < criteria.BirthdayMax)
                                                              && (criteria.GenderId == null ||
                                                                  e.GenderId == criteria.GenderId)
                                                              && (criteria.MaritalStatusId == null
                                                                  || e.AdditionalInfo.MaritalStatusId ==
                                                                  criteria.MaritalStatusId)
                                                              && (criteria.ReligionId == null
                                                                  || e.AdditionalInfo.ReligionId ==
                                                                  criteria.ReligionId)
                                                              && (criteria.AlcoholOpinionId == null
                                                                  || e.AdditionalInfo.AlcoholAttitudeId ==
                                                                  criteria.AlcoholOpinionId)
                                                              && (criteria.SmokingOpinionId == null
                                                                  || e.AdditionalInfo.SmokingAttitudeId ==
                                                                  criteria.SmokingOpinionId)
                                                              && (!criteria.Interests.Any() || e.UserInterests.All(
                                                                      d => new
                                                                              HashSet<int>(
                                                                                  criteria.Interests.Select(c =>
                                                                                      c.Id))
                                                                          .Contains(d.InterestId))))
                    .Skip((page - 1) * length)
                    .Take(length)
                    .Select(UserDto.Selector(issuerId))
                    .ToList();
            return foundUsers;
        }

        public IEnumerable<UserDto> TrendyUsers(int issuerId, int page, int length)
        {
            return _friendyContext
                .Set<Models.User>()
                .OrderByDescending(e => e.FriendRequestReceiver.Count)
                .Select(UserDto.Selector(issuerId))
                .Skip((page - 1) * length)
                .Take(length);
        }

        public async Task<IEnumerable<UserDto>> FindByKeywordAsync(string keyword, int issuerId, int page, int length)
        {
            var val = await _friendyContext.Set<Models.User>()
                .Where(e => (e.Name + " " + e.Surname).Contains(keyword))
                .Skip((page - 1) * length)
                .Take(length)
                .Select(UserDto.Selector(issuerId))
                .ToListAsync();
            return val;
        }
    }
}