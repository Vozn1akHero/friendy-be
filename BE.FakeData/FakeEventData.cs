using System;
using System.Collections;
using System.Collections.Generic;
using BE.Models;

namespace BE.FakeData
{
    public static class FakeEventData
    {
        public static Event CreateById(int id, int creatorId) => new Event()
        {
            Id = id,
            Title = "test title for "+id,
            Description = "test desc for "+id,
            Street = "test street for "+id,
            StreetNumber = "test st. num. for "+id,
            EntryPrice = id,
            ParticipantsAmount = id,
            Avatar = null,
            Background = null,
            Date = DateTime.Now, 
            CreatorId = id,
            CityId = id
        };

        public static IEnumerable<Event> CreateListById(int id, int creatorId, int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return CreateById(id, creatorId);
            }
        }
    }
}