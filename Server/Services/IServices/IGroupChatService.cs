using Server.Models;
using Shared.DTOs.Request.Chat;

namespace Server.Services.IServices;

public interface IGroupChatService : IChatService<GroupChatRequest> 
{
    Task<bool> AddUserToGroup(int chatId, ApplicationUser user);
    Task<bool> RemoveUserFromGroup(int chatId, ApplicationUser user);
}
