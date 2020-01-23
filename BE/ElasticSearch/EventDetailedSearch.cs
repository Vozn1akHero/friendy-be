using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.ElasticSearch;
using BE.Models;
using Nest;

namespace BE.Services.Elasticsearch
{
    public interface IEventDetailedSearch
    {
        IEnumerable<EventDto> SearchByCriteria(EventSearchDto eventSearchDto);
    }
    
    public class EventDetailedSearch : IEventDetailedSearch
    {
        public EventDetailedSearch(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }   
        private readonly ElasticClient _client;
        
        public IEnumerable<EventDto> SearchByCriteria(EventSearchDto eventSearchDto)
        {
            var searchResponse = _client.Search<EventDto>(s => s
                .Query(q => q
                    .Wildcard(m => m
                        .Field(f => f.Title)
                        .Value(eventSearchDto.Title+"*")
                    )
                )
            );
            var eventDtos = searchResponse.Documents;
            return eventDtos;
        }
    }
}