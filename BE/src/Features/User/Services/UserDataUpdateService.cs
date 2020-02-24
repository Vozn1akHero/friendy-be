using System;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.User
{
    public interface IUserDataUpdateService
    {
        Task UpdateEducationDataById(int id, int? educationId);
        Task UpdateBasicDataById(int id, string name, string surname, DateTime birthday);

        Task UpdateAdditionalDataById(int id, int? religionId, int? alcoholAttitudeId,
            int? maritalStatusId, int? smokingAttitudeId);

        Task UpdateEmailByIdAsync(int id, string email);
        Task UpdatePasswordByIdAsync(int id, string password);
    }
    
    public class UserDataUpdateService : IUserDataUpdateService
    {
        private IRepositoryWrapper _repository;
        private FriendyContext _friendyContext;

        public UserDataUpdateService(IRepositoryWrapper repository, FriendyContext friendyContext)
        {
            _repository = repository;
            _friendyContext = friendyContext;
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

    }
}