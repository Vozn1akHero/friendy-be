using BE.Dtos;
using BE.ElasticSearch;
using Nest;

namespace BE.Services.Elasticsearch
{
    public interface IUserDataIndexing
    {
        void Create(ExtendedUserDto extendedUserDto);
    }
    
    public class UserDataIndexing : IUserDataIndexing
    {
        public UserDataIndexing(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }   
        private readonly ElasticClient _client;

        public void Create(ExtendedUserDto extendedUserDto)
        {
            var ndexResponse = _client.CreateDocument(extendedUserDto);
        }
    }
}