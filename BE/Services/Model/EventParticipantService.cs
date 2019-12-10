using System.Threading.Tasks;
using AutoMapper;
using BE.Interfaces;

namespace BE.Services.Model
{
    public interface IEventParticipantService
    {
        
    }
    public class EventParticipantService : IEventParticipantService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public EventParticipantService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task LeaveEvent(int userId, int eventId)
        {
            
        }
    }
}