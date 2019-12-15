using System;
using System.Collections.Generic;
using BE.Dtos;
using BE.ElasticSearch;
using Nest;

namespace BE.Services.Elasticsearch
{
    public interface IUserDataUpdating
    {
        void UpdateBasicById(int id, string name, string surname, DateTime birthday);
        void UpdateEducationData(int id, int educationId);
        void UpdateInterestsById(int id, IEnumerable<int> userInterests);
    }
    
    public class UserDataUpdating : IUserDataUpdating
    {
        public UserDataUpdating(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }   
        private readonly ElasticClient _client;

        public void UpdateBasicById(int id, string name, string surname, DateTime birthday)
        {
            dynamic updatedDoc = new System.Dynamic.ExpandoObject();
            updatedDoc.name = name;
            updatedDoc.surname = surname;
            updatedDoc.birthday = birthday;

            _client.Update<ExtendedUserDto, object>(id, u => u.Doc(updatedDoc));

            /*_client.UpdateByQuery<ExtendedUserDto>(u => u
                .Query(q => q
                    .Term(f => f.Id, id)
                )
                .Script($"name = {name}, surname = {surname}, birthday = {birthday}")
            );*/
        }

        public void UpdateEducationData(int id, int educationId)
        {
            
            //es
        }

        public void UpdateInterestsById(int id, IEnumerable<int> userInterests)
        {
           
            //es
        }
    }
}