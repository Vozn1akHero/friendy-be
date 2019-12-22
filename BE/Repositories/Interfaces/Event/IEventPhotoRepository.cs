using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.PhotoDtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces
{
    public interface IEventPhotoRepository : IRepositoryBase<EventImage>
    {
        Task<IEnumerable<EventImage>> GetRange(int eventId, int startIndex, int 
        length);
        Task Add(int eventId, int photoId);
        Task<int> GetPicturesAmountByEventId(int eventId);
    }
}