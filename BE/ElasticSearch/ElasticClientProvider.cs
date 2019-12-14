using BE.Dtos;
using BE.Dtos.EventDtos;
using BE.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace BE.ElasticSearch
{
    public class ElasticClientProvider
    {
        public ElasticClientProvider(IOptions<ElasticConnectionSettings> settings)
        {
            ConnectionSettings connectionSettings = new ConnectionSettings(new System.Uri(settings.Value.ClusterUrl));

            connectionSettings
                .EnableDebugMode()
                .PrettyJson()
                .DefaultIndex(settings.Value.DefaultIndex)
                .DefaultMappingFor<EventDto>(m => m
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
                .DefaultMappingFor<ExtendedUserDto>(m => m.PropertyName(p => p.Id, "id")
                    .PropertyName(p => p.Name, "name")
                    .PropertyName(p => p.City, "city")
                    .PropertyName(p => p.Surname, "surname")
                    .PropertyName(p => p.GenderId, "gender_id")
                    .PropertyName(p => p.Birthday, "birthday")
                    .PropertyName(p => p.Avatar, "avatar")
                    .PropertyName(p => p.EducationId, "education_id")
                    .PropertyName(p => p.MaritalStatusId, "marital_status_id")
                    .PropertyName(p => p.ReligionId, "religionId")
                    .PropertyName(p => p.AlcoholAttitudeId, "alcohol_attitude_id")
                    .PropertyName(p => p.SmokingAttitudeId, "smoking_attitude_id")
                    .PropertyName(p => p.UserInterests, "user_interests"));

            Client = new ElasticClient(connectionSettings);
        }

        public ElasticClient Client { get; }
    }
}