using Server.Models;
using Server.Models.Chats;

namespace Server.Repositories.IRepositories;

public interface IChatRepository<T> : IRepository<T> where T : BaseChat
{
    public Task<List<T>> GetByUser(ApplicationUser User);
}
