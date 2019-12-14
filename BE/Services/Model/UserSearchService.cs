using System.Collections.Generic;
using BE.Dtos;
using BE.Dtos.FriendDtos;
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
        public UserSearchService(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }   
        private readonly ElasticClient _client;
        
        public IEnumerable<ExtendedUserDto> SearchByCriteria(UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var searchResponse = _client.Search<ExtendedUserDto>(s => s
                .Query(q => q
                    .Wildcard(m => m
                        .Field(f => f.Name)
                        .Value(usersLookUpCriteriaDto.Name+"*")
                    )
                )
            );
            var users = searchResponse.Documents;
            return users;
        }
    }
}