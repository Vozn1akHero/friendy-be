using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Features.Photo.Dtos
{
    public class EventPhotoDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int EventId { get; set; }

        public static Expression<Func<EventImage, EventPhotoDto>> Selector
        {
            get
            {
                return e => new EventPhotoDto()
                {
                    Id = e.Image.Id,
                    EventId = e.Event.Id,
                    Path = e.Image.Path
                };
            }
        }
    }
}
