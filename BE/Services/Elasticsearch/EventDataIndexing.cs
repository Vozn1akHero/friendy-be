using BE.Dtos.EventDtos;
using BE.ElasticSearch;
using BE.Models;
using Nest;

namespace BE.Services.Elasticsearch
{
    public interface IEventDataIndexing
    {
        void Create(Event @event);
    }
    
    public class EventDataIndexing : IEventDataIndexing
    {
        public EventDataIndexing(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }   
        private readonly ElasticClient _client;

        public void Create(Event @event)
        {
            var ndexResponse = _client.CreateDocument(@event);
        }
    }
}