using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;

namespace BE.Services.Model
{
    public interface IUserDataService
    {
        Task<ExtendedUserDto> GetExtendedById(int id);
    }
    
    public class UserDataService : IUserDataService
    {
        private IRepositoryWrapper _repository;

        public UserDataService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<ExtendedUserDto> GetExtendedById(int id)
        {
            var userEx = await _repository.User.GetSelectedFieldsById(id, e => new ExtendedUserDto
            {
                Id = e.Id,
                City = e.City,
                Name = e.Name,
                Surname = e.Surname,
                GenderId = e.Gender.Id,
                Birthday = e.Birthday,
                Avatar = e.Avatar,
                ProfileBg = e.ProfileBg,
                Status = e.Status,
                IsOnline = e.SessionNavigation.ConnectionStart != null & e
                .SessionNavigation.ConnectionEnd == null,
                EducationId = e.EducationId,
                MaritalStatusId = e.AdditionalInfo.MaritalStatus.Id,
                ReligionId = e.AdditionalInfo.Religion.Id,
                AlcoholAttitudeId = e.AdditionalInfo.AlcoholAttitude.Id,
                SmokingAttitudeId = e.AdditionalInfo.SmokingAttitude.Id,
                UserInterests = e.UserInterests.Select(b => new
                {
                    b.Interest.Id,
                    b.Interest.Title
                })
            });
            return userEx;
        }
        
        public async Task UpdateBasicById(int id, string name, string surname, DateTime birthday)
        {
            await _repository.User.UpdateBasicDataById(id, name, surname, birthday);
            //es
        }

        public async Task UpdateEducationData(int id, Education education)
        {
            await _repository.User.UpdateEducationDataById(id, education);
            //es
        }

        public async Task UpdateInterestsById(int id, IEnumerable<UserInterests> userInterests)
        {
            await _repository.User.UpdateInterestsById(id, userInterests);
            //es
        }
    }
}