using System.Collections.Generic;
using BE.Features.User.Dtos;
using Nest;

namespace BE.ElasticSearch
{
    public interface IUserDetailedSearch
    {
        IEnumerable<ExtendedUserDto> SearchByCriteria(
            UsersLookUpCriteriaDto eventSearchDto);
    }

    public class UserDetailedSearch : IUserDetailedSearch
    {
        private readonly ElasticClient _client;

        public UserDetailedSearch(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }

        public IEnumerable<ExtendedUserDto> SearchByCriteria(
            UsersLookUpCriteriaDto eventSearchDto)
        {
            var searchResponse = _client.Search<ExtendedUserDto>(s => s
                .Query(q => q
                    .Wildcard(m => m
                        .Field(f => f.Name)
                        .Value(eventSearchDto.Name + "*")
                    )
                )
            );
            var users = searchResponse.Documents;
            return users;
        }
    }
}