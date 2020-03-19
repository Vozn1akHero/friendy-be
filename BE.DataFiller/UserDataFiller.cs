using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Features.User.Dtos;
using BE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BE.DataFiller
{
    public class UserDataFiller
    {
        private FriendyContext _friendyContext = new FriendyContext();
        
        public async Task ExecuteAsync()
        {
            using (StreamReader r = new StreamReader("user-data.json"))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
                var maleNames = items["male_names"].ToHashSet();
                var maleSurnames = items["male_surnames"].ToHashSet();
                var cities = _friendyContext.City.Count();
                
                    for (int i = 0; i < maleNames.Count; i++)
                    {
                        var randomNameIndex = new Random().Next(maleNames.Count-1);
                        var randomSurnameIndex = new Random().Next(maleSurnames.Count-1);
                        int cityId = new Random().Next(1, cities-1);
                        int bYear = new Random().Next(1960, 2001);
                        int bMonth = new Random().Next(1, 12);
                        int bDay = new Random().Next(1, 27);
                        string name = maleNames.ElementAt(randomNameIndex);
                        string surname = maleSurnames.ElementAt(randomSurnameIndex);
                        _friendyContext.User.Add(new User
                        {
                            Name = name,
                            Surname = surname,
                            CityId = cityId,
                            Email = $"{name.ToLower()}.{surname.ToLower()}{i}@gmail.com",
                            Password = Guid.NewGuid().ToString(),
                            Birthday = new DateTime(bYear, bMonth, bDay),
                            GenderId = 1,
                            EducationId = new Random().Next(1,3),
                            AdditionalInfo = new UserAdditionalInfo
                            {
                                ReligionId = new Random().Next(1,3),
                                MaritalStatusId = new Random().Next(1,2),
                                AlcoholAttitudeId = new Random().Next(1,2),
                                SmokingAttitudeId = new Random().Next(1,2)
                            },
                            Session = new Session()
                        });
                    }
                    await _friendyContext.SaveChangesAsync();
            }
        }
    }
}