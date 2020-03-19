using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BE.DataFiller
{
    internal class Region
    {
        public string RegionName { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }
    
    internal class City
    {
        public string Id { get; set; }
        public string TextSimple { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public IEnumerable<District> Districts { get; set; }
    }

    internal class District
    {
        public string TextDistrict { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
    }
    
    public class PolishCitiesFiller
    {
        private FriendyContext _friendyContext = new FriendyContext();

        public async Task ExecuteAsync()
        {
            using (StreamReader r = new StreamReader("cities.json"))
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                };
                string json = await r.ReadToEndAsync();
                var items = JsonConvert.DeserializeObject<IEnumerable<Region>>(json);
                foreach (var region in items)
                {
                    _friendyContext.Voivodeship.Add(new Voivodeship()
                    {
                        Title = region.RegionName,
                        City = region.Cities.Select(c=>new Models.City
                        {
                            Title = c.TextSimple,
                            Lat = c.Lat,
                            Lon = c.Lon,
                            District = c.Districts.Select(d=> new Models.District
                            {
                                Title = d.TextDistrict,
                                Lat = d.Lat,
                                Lon = d.Lon
                            }).ToList()
                        }).ToList()
                    });
                    await _friendyContext.SaveChangesAsync();
                }
            }
        }
    }
}