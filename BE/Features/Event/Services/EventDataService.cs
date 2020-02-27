using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Event.Dtos;
using BE.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BE.Features.Event.Services
{
    public interface IEventDataService
    {
        Task<EventDto> GetDtoById(int eventId);
        Task<NewAvatarDto> UpdateAvatarAsync(int eventId, IFormFile file);
        Task<NewBackgroundDto> UpdateBackgroundAsync(int eventId, IFormFile file);
        Task<EventDto> UpdateAndReturnDataById(int eventId, EventDataDto eventDataDto);
    }

    public class EventDataService : IEventDataService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public EventDataService(IRepositoryWrapper repository,
            IMapper mapper, IImageSaver imageSaver)
        {
            _repository = repository;
            _mapper = mapper;
            _imageSaver = imageSaver;
        }

        public async Task<EventDto> GetDtoById(int eventId)
        {
            var eventData = await _repository.Event.GetById(eventId, EventDto.Selector);
            //var eventDataDto = _mapper.Map<EventDto>(eventData);
            return eventData;
        }

        public async Task<NewAvatarDto> UpdateAvatarAsync(int eventId, IFormFile file)
        {
            var path = $"wwwroot/EventAvatar/{eventId}/{file.FileName}";
            await _imageSaver
                .SaveWithSpecifiedName(file, path);
            var newAvatar = new NewAvatarDto {Avatar = path};
            _repository.Event.UpdateById(eventId, newAvatar);
            await _repository.SaveAsync();
            return newAvatar;
        }

        public async Task<NewBackgroundDto> UpdateBackgroundAsync(int eventId,
            IFormFile file)
        {
            var path = $"wwwroot/EventBackground/{eventId}/{file.FileName}";
            await _imageSaver
                .SaveWithSpecifiedName(file, path);
            var newBg = new NewBackgroundDto {Background = path};
            _repository.Event.UpdateById(eventId, newBg);
            await _repository.SaveAsync();
            return newBg;
        }

        public async Task<EventDto> UpdateAndReturnDataById(int eventId,
            EventDataDto eventDataDto)
        {
            _repository.Event.UpdateById(eventId, eventDataDto);
            await _repository.SaveAsync();
            var eventData = await _repository.Event.GetById(eventId, EventDto.Selector);
            return eventData;
        }
    }
}