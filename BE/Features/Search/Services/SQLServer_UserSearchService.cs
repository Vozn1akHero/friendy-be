using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.User.Dtos;
using BE.Models;
using BE.Shared.Dtos;

namespace BE.Features.Search.Services
{
    public interface ISQLServer_UserSearchService
    {
        Task<IEnumerable<UserDto>> SearchByCriteriaAsync(
            UsersLookUpCriteriaDto criteria, int page, int issuerId);
        IEnumerable<UserDto> TrendyUsers(int issuerId, int page);
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
            (UsersLookUpCriteriaDto criteria, int page, int issuerId)
        {
            var length = 25;
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
                    /*.ProjectTo<FoundUserDto>(_mapper.ConfigurationProvider, new Dictionary<string, object>
                     {
                         {"issuerId", issuerId}
                     })*/
                    .ToList();
            return foundUsers;
        }

        public IEnumerable<UserDto> TrendyUsers(int issuerId, int page)
        {
            int size = 20;
            return _friendyContext
                .Set<Models.User>()
                .OrderByDescending(e => e.FriendRequestReceiver.Count)
                .Select(UserDto.Selector(issuerId))
                .Skip((page - 1) * size)
                .Take(size);
        }
    }
}