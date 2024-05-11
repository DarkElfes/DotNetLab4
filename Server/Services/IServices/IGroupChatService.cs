using Server.Models.Chats;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Services.IServices;

public interface IGroupChatService : IChatService<GroupChatRequest, GroupChatDTO> 
{
    Task<GroupChatDTO> LeaveFromChat(int chatId, string userId);

    Task<GroupChatDTO> AddUserAsync(int chatId, string ownerId, string userEmail);
    Task<GroupChatDTO> RemoveUserFromChat(int chatId, string ownerId, string userId);
}
