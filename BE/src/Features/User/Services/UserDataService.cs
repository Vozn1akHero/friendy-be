using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.UserDtos;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;
using BE.Models;

namespace BE.Features.User.Services
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

        Task<bool> CheckOldPasswordBeforeUpdateByUserIdAsync(int id,
            string password);

        Task<IEnumerable<UserForIndexingDto>> GetDataForElasticsearchIndex();
        Task<IEnumerable<Interest>> FindInterestsByTitle(string title);
    }

    public class UserDataService : IUserDataService
    {
        private readonly FriendyContext _friendyContext;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public UserDataService(IRepositoryWrapper repository,
            FriendyContext friendyContext, IMapper mapper)
        {
            _repository = repository;
            _friendyContext = friendyContext;
            _mapper = mapper;
        }

        public async Task<ExtendedUserDto> GetExtendedById(int id)
        {
            var userEx =
                await _repository.User.GetSelectedFieldsById(id,
                    ExtendedUserDto.Selector());
            return userEx;
        }

        public async Task UpdateBasicDataById(int id, string name, string surname,
            DateTime birthday)
        {
            var user = await _repository.User.GetByIdAsync(id);
            user.Name = name;
            user.Surname = surname;
            user.Birthday = birthday;
            _friendyContext.Entry(user).State = EntityState.Modified;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateEducationDataById(int id, int? educationId)
        {
            var userCur = await _repository.User.GetByIdAsync(id);
            userCur.EducationId = educationId;
            _friendyContext.Entry(userCur).State = EntityState.Modified;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateAdditionalDataById(int id, int? religionId,
            int? alcoholAttitudeId,
            int? maritalStatusId, int? smokingAttitudeId)
        {
            var userCur = await _repository.User.GetByIdAsync(id);
            userCur.AdditionalInfo.ReligionId = religionId;
            userCur.AdditionalInfo.AlcoholAttitudeId = alcoholAttitudeId;
            userCur.AdditionalInfo.MaritalStatusId = maritalStatusId;
            userCur.AdditionalInfo.SmokingAttitudeId = smokingAttitudeId;
            _friendyContext.Entry(userCur).State = EntityState.Modified;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdateEmailByIdAsync(int id, string email)
        {
            var userCur = await _repository.User.GetByIdAsync(id);
            userCur.Email = email;
            _friendyContext.Entry(userCur).State = EntityState.Modified;
            await _friendyContext.SaveChangesAsync();
        }

        public async Task UpdatePasswordByIdAsync(int id, string password)
        {
            if (password != null)
            {
                var userCur = await _repository.User.GetByIdAsync(id);
                userCur.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _friendyContext.Entry(userCur).State = EntityState.Modified;
                await _friendyContext.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckOldPasswordBeforeUpdateByUserIdAsync(int id,
            string password)
        {
            dynamic user =
                await _repository.User.GetWithSelectedFields(id, new[] {"Password"});
            var isOldPassCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isOldPassCorrect;
        }

        public async Task<IEnumerable<UserForIndexingDto>> GetDataForElasticsearchIndex()
        {
            var users = await _repository.User.GetAllAsync();
            var usersDtos = _mapper.Map<List<UserForIndexingDto>>(users);
            return usersDtos;
        }

        public async Task<IEnumerable<Interest>> FindInterestsByTitle(string title)
        {
            var userInterests = await _friendyContext.Interest.Where(e => e.Title
                    .ToLower().StartsWith(title.ToLower()))
                .ToListAsync();
            return userInterests;
        }

        public async Task UpdateInterestsById(int id,
            IEnumerable<UserInterests> userInterests)
        {
            await _repository.User.UpdateInterestsById(id, userInterests);
            //es
        }
    }
}