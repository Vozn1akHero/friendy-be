using System.Collections.Generic;
using BE.Dtos;
using BE.ElasticSearch;
using Nest;

namespace BE.Services.Elasticsearch
{
    public interface IUserDetailedSearch
    {
        IEnumerable<ExtendedUserDto> SearchByCriteria(UsersLookUpCriteriaDto eventSearchDto);
    }
    
    public class UserDetailedSearch : IUserDetailedSearch
    {
        public UserDetailedSearch(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }   
        private readonly ElasticClient _client;
        
        public IEnumerable<ExtendedUserDto> SearchByCriteria(UsersLookUpCriteriaDto eventSearchDto)
        {
            var searchResponse = _client.Search<ExtendedUserDto>(s => s
                .Query(q => q
                    .Wildcard(m => m
                        .Field(f => f.Name)
                        .Value(eventSearchDto.Name+"*")
                    )
                )
            );
            var users = searchResponse.Documents;
            return users;
        }
    }
}