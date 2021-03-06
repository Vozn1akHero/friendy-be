using System;
using System.Collections.Generic;
using System.Dynamic;
using BE.Features.User.Dtos;
using Nest;

namespace BE.ElasticSearch
{
    public interface IUserDataUpdating
    {
        void UpdateBasicById(int id, string name, string surname, DateTime birthday);
        void UpdateEducationData(int id, int educationId);
        void UpdateInterestsById(int id, IEnumerable<int> userInterests);
    }

    public class UserDataUpdating : IUserDataUpdating
    {
        private readonly ElasticClient _client;

        public UserDataUpdating(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }

        public void UpdateBasicById(int id, string name, string surname,
            DateTime birthday)
        {
            dynamic updatedDoc = new ExpandoObject();
            updatedDoc.name = name;
            updatedDoc.surname = surname;
            updatedDoc.birthday = birthday;

            _client.Update<ExtendedUserDto, object>(id, u => u.Doc(updatedDoc));
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