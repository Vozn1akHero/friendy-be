using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos;
using BE.ElasticSearch;
using BE.Models;
using Nest;

namespace BE.Services.Elasticsearch
{
    public interface IUserDataIndexing
    {
        void Create(User user);
        Task UpdateAvatar(int id, string path);
        Task UpdateEducation(int id, int? educationId);

        Task UpdateBasicData(int id,
            User user);

        Task UpdateAdditionalDataByIdAsync(int id,
            int? religionId, int? alcoholAttitudeId,
            int? maritalStatusId, int? smokingAttitudeId);

        Task Bulk(IEnumerable<UserForIndexingDto> userForIndexingDtos);
    }

    public class UserDataIndexing : IUserDataIndexing
    {
        public UserDataIndexing(ElasticClientProvider provider)
        {
            _client = provider.Client;
        }

        private readonly ElasticClient _client;

        public void Create(User user)
        {
            var extendedUserDto = new UserForIndexingDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                City = user.City,
                Avatar = user.Avatar,
                Birthday = user.Birthday,
                EducationId = null,
                GenderId = user.GenderId,
                MaritalStatusId = null,
                ReligionId = null,
                AlcoholAttitudeId = null,
                SmokingAttitudeId = null,
                UserInterests = null
            };
            var indexResponse = _client.CreateDocument(extendedUserDto);
        }

        public async Task UpdateInterests(int id,
            IEnumerable<UserInterestForElasticsearchDto> interests)
        {
            await _client.UpdateAsync<UserForIndexingDto, object>(new
                    DocumentPath<UserForIndexingDto>(id), e => e
                    .Index("users")
                    .Doc(new {Interests = interests})
                    .RetryOnConflict(3)
            );
        }

        public async Task UpdateBasicData(int id, User user)
        {
            await _client.UpdateAsync(new
                    DocumentPath<UserForIndexingDto>(id), e => e
                    .Index("users")
                    .Doc(new UserForIndexingDto
                    {
                        Name = user.Name,
                        Surname = user.Surname,
                        Birthday = user.Birthday
                    })
                    .RetryOnConflict(3)
            );
        }

        public async Task UpdateAdditionalDataByIdAsync(int id,
            int? religionId, int? alcoholAttitudeId, 
            int? maritalStatusId, int? smokingAttitudeId)
        {
            await _client.UpdateAsync(new
                    DocumentPath<UserForIndexingDto>(id), e => e
                    .Index("users")
                    .Doc(new UserForIndexingDto
                    {
                        MaritalStatusId = maritalStatusId,
                        ReligionId = religionId,
                        AlcoholAttitudeId = alcoholAttitudeId,
                        SmokingAttitudeId = smokingAttitudeId
                    })
                    .RetryOnConflict(3)
            );
        }

        public async Task UpdateEducation(int id, int? educationId)
        {
            await _client.UpdateAsync(new
                    DocumentPath<UserForIndexingDto>(id), e => e
                    .Index("users")
                    .Doc(new UserForIndexingDto
                    {
                        EducationId = educationId
                    })
                    .RetryOnConflict(3)
            );
        }

        public async Task UpdateAvatar(int id, string path)
        {
            await _client.UpdateAsync<UserForIndexingDto, object>(new
                    DocumentPath<UserForIndexingDto>(id), e => e
                    .Index("users")
                    .Doc(new { Avatar = path })
                    .RetryOnConflict(3)
            );
        }

        public async Task Bulk(IEnumerable<UserForIndexingDto> userForIndexingDtos)
        {
            foreach (var userForIndexingDto in userForIndexingDtos)
            {
                await _client.IndexAsync(userForIndexingDto, e => e
                    .Index("users")
                    .Id(userForIndexingDto.Id));
            }
        }
    }
}