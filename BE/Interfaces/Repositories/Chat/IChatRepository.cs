using System.Threading.Tasks;

namespace BE.Interfaces.Repositories.Chat
{
    public interface IChatRepository: IRepositoryBase<Models.Chat>
    {
        Task<Models.Chat> AddNewAfterFriendAdding();
    }
}