using System.Collections.Generic;
using System.Linq;
using BE.Models;

namespace BE.Features.Helpers
{
    public interface ILocationService
    {
        IEnumerable<City> GetByKeyword(string keyword);
    }

    public class LocationService : ILocationService
    {
        private FriendyContext _friendyContext;

        public LocationService(FriendyContext friendyContext)
        {
            _friendyContext = friendyContext;
        }

        public IEnumerable<City> GetByKeyword(string keyword)
        {
            var foundCities = _friendyContext.City.Where(e => e.Title.StartsWith(keyword)).ToList();
            return foundCities;
        }
    }
}