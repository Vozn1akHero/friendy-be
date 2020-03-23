using System;
using System.Collections.Generic;

using System.Linq.Expressions;

using BE.Models;

namespace BE.Features.Photo
{
    public class UserPhotoDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int UserId { get; set; }

        public static Expression<Func<UserImage, UserPhotoDto>> Selector
        {
            get
            {
                return e => new UserPhotoDto()
                {
                    Id = e.Image.Id,
                    Path = e.Image.Path,
                    UserId = e.User.Id
                };
            }
        }
    }
}
