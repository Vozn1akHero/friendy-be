using System;
using BE.Features.Event.Dtos;
using BE.Features.User.Dtos;
using Microsoft.Extensions.Options;
using Nest;

namespace BE.ElasticSearch
{
    public class ElasticClientProvider
    {
        public ElasticClientProvider(IOptions<ElasticConnectionSettings> settings)
        {
            var connectionSettings =
                new ConnectionSettings(new Uri(settings.Value.ClusterUrl));

            connectionSettings
                .EnableDebugMode()
                .PrettyJson()
                .DefaultIndex(settings.Value.DefaultIndex)
                .DefaultMappingFor<EventDto>(m => m
                    .IndexName("events")
                    .PropertyName(p => p.Id, "id")
                    .PropertyName(p => p.Title, "title")
                    .PropertyName(p => p.City, "city")
                    .PropertyName(p => p.Description, "description")
                    .PropertyName(p => p.AvatarPath, "avatar")
                    .PropertyName(p => p.BackgroundPath, "background")
                    .PropertyName(p => p.Street, "street")
                    .PropertyName(p => p.StreetNumber, "street_number")
                    .PropertyName(p => p.EntryPrice, "entry_price")
                    .PropertyName(p => p.ParticipantsAmount, "participants_amount")
                    .PropertyName(p => p.Date, "date")
                    .PropertyName(p => p.CreatorId, "creator_id")
                )
                .DefaultMappingFor<ExtendedUserDto>(m => m
                    .IndexName("users").PropertyName(p => p.Id, "id")
                    .PropertyName(p => p.Name, "name")
                    .PropertyName(p => p.City, "city")
                    .PropertyName(p => p.Surname, "surname")
                    .PropertyName(p => p.GenderId, "genderId")
                    .PropertyName(p => p.Birthday, "birthday")
                    .PropertyName(p => p.Avatar, "avatar")
                    .PropertyName(p => p.EducationId, "educationId")
                    .PropertyName(p => p.MaritalStatusId, "maritalStatusId")
                    .PropertyName(p => p.ReligionId, "religionId")
                    .PropertyName(p => p.AlcoholAttitudeId, "alcoholAttitudeId")
                    .PropertyName(p => p.SmokingAttitudeId, "smokingAttitudeId")
                    .PropertyName(p => p.UserInterests, "userInterests"));

            Client = new ElasticClient(connectionSettings);
        }

        public ElasticClient Client { get; }
    }
}