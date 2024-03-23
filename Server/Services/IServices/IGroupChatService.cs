using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Services.IServices;

public interface IGroupChatService : IChatService<GroupChatRequest> 
{
    Task<BaseChatDTO> LeaveFromChat(int chatId, string userId);

    Task<BaseChatDTO> AddUserToChat(int chatId, string ownerId, string userId);
    Task<BaseChatDTO> RemoveUserFromChat(int chatId, string ownerId, string userId);
}
