using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.ElasticSearch;
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
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Title)
                        .Query(eventSearchDto.Title)
                    )
                )
            );
            var eventDtos = searchResponse.Documents;
            return eventDtos;
        }
    }
}