using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.ChatDtos;
using BE.Interfaces;

namespace BE.Helpers
{
    interface ILastMessageBuilder
    {
        Task AddBasic();
    }
    
    public class LastMessageBuilder : ILastMessageBuilder
    {
        private ChatLastMessageDto chatLastMessageDto { get; set; }
        private IRepositoryWrapper _repositoryWrapper;

        public LastMessageBuilder(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        
        public async Task AddBasic(int userId, int startIndex, int length)
        {
            var messages = await _repositoryWrapper
                .ChatMessages
                .GetLastChatMessageRangeByReceiverId(userId, startIndex, length);
            var ids = messages.Select(e => e.ChatId);
            foreach (var id in ids)
            {
                
            }
        }

        public Task AddBasic()
        {
            throw new System.NotImplementedException();
        }
    }
}