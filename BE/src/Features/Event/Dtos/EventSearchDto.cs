using System;
using Nest;

namespace BE.Features.Event.Dto
{
    [ElasticsearchType(Name = "event")]
    public class EventSearchDto
    {
        [Keyword] public string Title { get; set; }

        [Keyword] public string Street { get; set; }

        [Keyword] public string StreetNumber { get; set; }

        [Keyword] public string City { get; set; }

        [Keyword] public int ParticipantsMin { get; set; }

        [Keyword] public int ParticipantsMax { get; set; }

        [Keyword] public int PriceMin { get; set; }

        [Keyword] public int PriceMax { get; set; }

        [Date] public DateTime Date { get; set; }
    }
}