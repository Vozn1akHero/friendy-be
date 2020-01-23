using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.EventDtos;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;

namespace BE.Services.Model
{
    public interface IEventParticipantService
    {
        Task<IEnumerable<BannedEventParticipantDto>>
            FindBannedUsersByEventIdAsync(int eventId);

        void BanParticipant(int id, int eventId);
        void UnbanParticipant(int id, int eventId);
    }
    public class EventParticipantService : IEventParticipantService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public EventParticipantService(IRepositoryWrapper repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task LeaveEvent(int userId, int eventId)
        {
            
        }

        public async Task<IEnumerable<BannedEventParticipantDto>> 
            FindBannedUsersByEventIdAsync(int eventId)
        {
            var bu = await _repository.EventBannedUsers
                .FindEventBannedUsersByEventIdAsync(eventId);
            var res = bu.Select(e => new BannedEventParticipantDto
            {
                Id = e.Id,
                Name = e.User.Name,
                Surname = e.User.Surname,
                AvatarPath = e.User.Avatar
            });
            return res;
        }

        public void UnbanParticipant(int id, int eventId)
        {
            _repository.EventBannedUsers.RemoveByUserIdAndEventId(id, eventId);
            _repository.Save();
        }

        public void BanParticipant(int id, int eventId)
        {
            /*using(var unitOfWork = new RepositoryWrapper())
            using(var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    await _repository.EventBannedUsers.Add(new EventBannedUsers
                    {
                        EventId = eventId,
                        UserId = id
                    });
                    await _repository
                        .EventParticipants
                        .DeleteByUserIdAndEventId(id, eventId);
                    _repository.Save();
                    transaction.Commit();
                    return 1;
                } 
                catch(Exception)
                {
                    transaction.Rollback();
                    return 0;
                }
            }*/
            _repository.EventBannedUsers.Add(new EventBannedUsers
            {
                EventId = eventId,
                UserId = id
            });
            _repository
                .EventParticipants
                .DeleteByUserIdAndEventId(id, eventId);
            _repository.Save();
        }
    }
}