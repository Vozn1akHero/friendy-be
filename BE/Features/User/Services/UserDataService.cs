using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.User.Dtos;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.User.Services
{
    public interface IUserDataService
    {
        Task<ExtendedUserDto> GetExtendedById(int id);
        Task<bool> CheckOldPasswordBeforeUpdateByUserIdAsync(int id,
            string password);
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

       
        public async Task<bool> CheckOldPasswordBeforeUpdateByUserIdAsync(int id,
            string password)
        {
            dynamic user =
                await _repository.User.GetWithSelectedFields(id, new[] {"Password"});
            var isOldPassCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isOldPassCorrect;
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