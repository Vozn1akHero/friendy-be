using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Helpers;
using BE.Interfaces;
using BE.Interfaces.Repositories;
using BE.Models;
using BE.Services;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EventRepository : RepositoryBase<Models.Event>, IEventRepository
    {
        private readonly IAvatarConverterService _userAvatarConverterService;
        
        public EventRepository(FriendyContext friendyContext,
            IAvatarConverterService userAvatarConverterService) : base(friendyContext)
        {
            _userAvatarConverterService = userAvatarConverterService;
        }


        public async Task<List<Models.Event>> GetExampleEventsByUserId(int userId)
        {
/*
            var events = await FindByCondition(e => e.EventParticipants.Part)
                .Include(e => e.UserEvents)
                .ToListAsync();
            
            */

            return null;
        }

        public async Task<Models.Event> GetById(int id)
        {
            return await FindByCondition(e => e.Id == id)
                .SingleOrDefaultAsync();
        }
        
        public async Task<object> GetWithSelectedFields(int id, string[] selectedFields)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            return DynamicLinqStatement.ExtractSpecifiedFields<Models.Event>(obj, selectedFields);
        }

        public async Task<string> GetAvatarPathByEventIdAsync(int id)
        {
            return await FindByCondition(e => e.Id == id).Select(e => e.Avatar).SingleOrDefaultAsync();
        }
        
        public async Task UpdateAvatarAsync(string path, int id)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            obj.Avatar = path;
            await SaveAsync();
        }

        public async Task UpdateBackgroundAsync(string path, int id)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            obj.Background = path;
            await SaveAsync();
        }
    }
}