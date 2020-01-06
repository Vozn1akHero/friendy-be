using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;

namespace BE.Services.Model
{
    public interface IUserDataService
    {
        Task<ExtendedUserDto> GetExtendedById(int id);
        Task UpdateEducationDataById(int id, int? educationId);
        Task UpdateBasicDataById(int id, string name, string surname, DateTime birthday);

        Task UpdateAdditionalDataById(int id, int? religionId, int? alcoholAttitudeId,
            int? maritalStatusId, int? smokingAttitudeId);
        Task UpdateEmailByIdAsync(int id, string email);
        Task UpdatePasswordByIdAsync(int id, string password);
        Task<IEnumerable<UserForIndexingDto>> GetDataForElasticsearchIndex();
    }

    public class UserDataService : IUserDataService
    {
        private IRepositoryWrapper _repository;
        private FriendyContext _friendyContext;
        private IMapper _mapper;

        public UserDataService(IRepositoryWrapper repository,
            FriendyContext friendyContext, IMapper mapper)
        {
            _repository = repository;
            _friendyContext = friendyContext;
            _mapper = mapper;
        }

        public async Task<ExtendedUserDto> GetExtendedById(int id)
        {
            var userEx = await _repository.User.GetSelectedFieldsById(id, e =>
                new ExtendedUserDto
                {
                    Id = e.Id,
                    City = e.City,
                    Name = e.Name,
                    Surname = e.Surname,
                    Email = e.Email,
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

        public async Task UpdateBasicDataById(int id, string name, string surname,
            DateTime birthday)
        {
            var user = await _repository.User.GetByIdAsync(id);
            user.Name = name;
            user.Surname = surname;
            user.Birthday = birthday;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateEducationDataById(int id, int? educationId)
        {
            var user = await _repository.User.GetByIdAsync(id);
            user.EducationId = educationId;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateAdditionalDataById(int id, int? religionId,
            int? alcoholAttitudeId,
            int? maritalStatusId, int? smokingAttitudeId)
        {
            var user = await _repository.User.GetByIdAsync(id);
            user.AdditionalInfo.ReligionId = religionId;
            user.AdditionalInfo.AlcoholAttitudeId = alcoholAttitudeId;
            user.AdditionalInfo.MaritalStatusId = maritalStatusId;
            user.AdditionalInfo.SmokingAttitudeId = smokingAttitudeId;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateEmailByIdAsync(int id, string email)
        {
            var userCur = await _repository.User.GetByIdAsync(id);
            userCur.Email = email;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdatePasswordByIdAsync(int id, string password)
        {
            if (password != null)
            {
                var userCur = await _repository.User.GetByIdAsync(id);
                userCur.Password = BCrypt.Net.BCrypt.HashPassword(password);
                await _friendyContext.SaveChangesAsync();
            }
        }

        public async Task UpdateInterestsById(int id,
            IEnumerable<UserInterests> userInterests)
        {
            await _repository.User.UpdateInterestsById(id, userInterests);
            //es
        }

        public async Task<IEnumerable<UserForIndexingDto>> GetDataForElasticsearchIndex()
        {
            var users = await _repository.User.GetAllAsync();
            var usersDtos = _mapper.Map<List<UserForIndexingDto>>(users);
            return usersDtos;
        }
    }
}